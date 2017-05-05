using Cayci.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Cayci.Helpers
{
    public class ProxyHelper
    {
        private readonly static HttpClient _client;
        static ProxyHelper()
        {
            _client = new HttpClient()
            {
                BaseAddress = new Uri(ConfigurationManager.AppSettings["apiaddress"])
            };
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public object Call<TContract, TResult>(Expression<Func<TContract, TResult>> function) where TContract : IContractBase
        {
            var type = typeof(TContract);
            var mainAttr = type.GetCustomAttributes(false).OfType<ApiRoute>().FirstOrDefault();
            if (mainAttr == null)
                throw new MemberAccessException("ApiRoute attribute is required for contract");
            var methodAttr = GetMethodAttribute(function, type);
            var methodParameters = GetMethodParameters(type, methodAttr);
            var httpMethod = GetMethod(methodAttr.Type);
            var path = $"{mainAttr.Path}/{methodAttr.Path}";
            var parameters = ((MethodCallExpression)function.Body).Arguments;
            var message = new HttpRequestMessage();
            message.Method = httpMethod;
            if (methodAttr.Type != MethodType.Get)
            {
                
                if (!parameters.Any())
                {
                    throw new Exception("At least send one parameter to post this action");
                }

                var hasComplex = parameters.Any(i => i.Type.IsClass && i.Type.Name != typeof(string).Name);
                if (parameters.Count > 1 && hasComplex)
                {
                    throw new Exception("Only one complex type can be sent to api");
                }
                else if (parameters.Count == 1 && hasComplex)
                {
                    var lambda = Expression.Lambda(parameters.First(), function.Parameters);
                    var compile = lambda.Compile();
                    object value = compile.DynamicInvoke(new object[1]);
                    var json = JsonConvert.SerializeObject(value);
                    message.RequestUri = new Uri(path, UriKind.Relative);
                    message.Content = new StringContent(json, Encoding.UTF8, "application/json");
                }
                else
                {
                    var queryString = new Dictionary<string, string>();
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        var param = parameters[i];
                        var methodparameter = methodParameters[i];
                        var lambda = Expression.Lambda(param, function.Parameters);
                        var compile = lambda.Compile();
                        var value = compile.DynamicInvoke(new string[1]);
                        queryString.Add(methodparameter, value.ToString());
                    }
                    var qsData = string.Empty;
                    foreach (var qs in queryString)
                    {
                        qsData += $"{qs.Key}={qs.Value}&";
                    }
                    path = path + "?" + qsData.Trim('&');
                    message.RequestUri = new Uri(path.Trim('?'), UriKind.Relative);
                }
            }
            else
            {
                var queryString = new Dictionary<string, string>();
                for (int i = 0; i < parameters.Count; i++)
                {
                    var param = parameters[i];
                    var methodparameter = methodParameters[i];
                    var lambda = Expression.Lambda(param, function.Parameters);
                    var compile = lambda.Compile();
                    var value = compile.DynamicInvoke(new string[1]);
                    queryString.Add(methodparameter, value.ToString());
                }
                var qsData = string.Empty;
                foreach (var qs in queryString)
                {
                    qsData += $"{qs.Key}={qs.Value}&";
                }
                path = path + "?" + qsData.Trim('&');
                message.RequestUri = new Uri(path.Trim('?'), UriKind.Relative);
            }
            var clientResponse = _client.SendAsync(message).Result;
            if (clientResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var responseResult = clientResponse.Content.ReadAsStringAsync().Result;
                var returnType = typeof(TResult);
                if (returnType.IsGenericType)
                {
                    var genericParameter = returnType.GenericTypeArguments.First();
                    return JsonConvert.DeserializeObject(responseResult, genericParameter);
                }
                return null;
            }
            else
                return null;
        }

        private List<string> GetMethodParameters(Type type, ApiRoute attribute)
        {
            var list = new List<string>();
            var methods = type.GetMethods().Where(i => i.GetCustomAttributes(false).OfType<ApiRoute>() != null);
            var found = false;
            foreach (var method in methods)
            {
                var attr = method.CustomAttributes.First();
                var value = attr.NamedArguments.FirstOrDefault(i => i.TypedValue.Value.ToString() == attribute.Path).TypedValue.Value;
                if (value == null)
                    continue;
                if (value.ToString() != attribute.Path)
                    continue;
                found = true;
                var parameters = method.GetParameters();
                foreach (var p in parameters)
                {
                    list.Add(p.Name);
                }
            }

            if (!found)
                throw new Exception("Method cannot be found");
            return list;
        }

        private static ApiRoute GetMethodAttribute<TContract, TResult>(Expression<Func<TContract, TResult>> function, Type type) where TContract : IContractBase
        {
            var methodName = (function.Body as MethodCallExpression).Method.Name;
            var methodInfo = type.GetMethods().FirstOrDefault(i => i.Name == methodName);
            var methodAttr = methodInfo.GetCustomAttributes(false).OfType<ApiRoute>().FirstOrDefault();
            if (methodAttr == null)
                throw new MemberAccessException("ApiRoute attribute is required for method");
            return methodAttr;
        }

        private static HttpMethod GetMethod(MethodType type)
        {
            HttpMethod method = null;
            switch (type)
            {
                case MethodType.NotSet:
                    throw new Exception("Method type should be defined as a known type");
                case MethodType.Post:
                    method = HttpMethod.Post;
                    break;
                case MethodType.Get:
                    method = HttpMethod.Get;
                    break;
                case MethodType.Put:
                    method = HttpMethod.Put;
                    break;
                case MethodType.Delete:
                    method = HttpMethod.Delete;
                    break;
            }

            return method;
        }
    }
}
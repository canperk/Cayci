using Cayci.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cayci.Entities
{
    public class ApiResult
    {
        public ApiResult(string message = "", ApiState state = ApiState.Success)
        {
            Message = message;
            State = state;
        }
        public string Message { get; set; }
        public ApiState State { get; set; }
    }

    public class ApiResult<T> : ApiResult
    {
        public ApiResult(T result, string message = "", ApiState state = ApiState.Success) : base(message, state)
        {
            Result = result;
        }

        public T Result { get; set; }
    }
}

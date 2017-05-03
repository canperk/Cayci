var SGB = SGB || {};
(function (w, $) {
    SGB.Request = {};
    SGB.Request.GetData = function (url, successCallback) {
        $.ajax({
            "url": url,
            "contentType": "application/json",
            "type": "GET"
        }).success(function (data) {
            var d = JSON.parse(data);
            successCallback(d);
        }).error(function (err) {
            SGB.OnError(err);
        });
    }
    SGB.Request.PostData = function (url, data, successCallback) {
        var d = JSON.stringify(data);
        $.ajax({
            "url": url,
            "contentType": "application/json",
            "type": "POST",
            "data": d
        }).success(function (data) {
            successCallback(data);
        }).error(function (err) {
            SGB.OnError(err);
        });
    }

    SGB.OnError = function (error) {
        SGB.Notify.Error("Bir Hata Oluştu", "Mesaj :" + error.status + ","+ error.statusText);
    }

    SGB.Notify = {};
    SGB.Notify.Success = function (title, message) {
        Notify(title, message, "check", "success");
    }
    SGB.Notify.Error = function (title, message) {
        Notify(title, message, "times", "alert");
    }
    SGB.Notify.Warning = function (title, message) {
        Notify(title, message, "exclamation", "warning");
    }

    SGB.Notify.ShowApiResult = function (result) {
        var apiResult = JSON.parse(result);
        var message = "";
        var type = "info"; 
        var icon = "info"; 
        
        if (!apiResult.State) {
            type = "info";
            icon = "info";
            message = "Bir işleminiz sonuçlandı";
        }
        else if (apiResult.State === 1) {
            type = "success";
            icon = "check";
            message = "İşlem başarıyla tamamlandı";
        }
        else if (apiResult.State === 2) {
            type = "warning";
            icon = "exclamation"
            message = "İşlem beklemedik bir şekilde sonuçlandı";
        }
        else if (apiResult.State === 3) {
            type = "alert";
            icon = "times";
            message = "Bir hata oluştu";
        }
        if (apiResult.Message)
            message = apiResult.Message;
        Notify("Bildirim Sistemi", message, icon, type);
    }

    function Notify(title, message, icon, type) {
        $.Notify({
            caption: title,
            content: message,
            icon: "<span class='fa fa-"+ icon +"'></span>",
            type: type
        });
    }

    SGB.SignalR = {};
    SGB.SignalR.Connection = $.connection.hub;
    SGB.SignalR.Hub = $.connection.Cayci;
    if (SGB.SignalR.Hub) {
        SGB.SignalR.HubClient = $.connection.Cayci.client;
        SGB.SignalR.HubClient.addNewRequest = function () {
            if (!SGB.ViewModels && !SB.ViewModels.TileViewModel) {
                console.log("1 yeni istek");
                return;
            }
            var c = SGB.ViewModels.TileViewModel.count();
            SGB.ViewModels.TileViewModel.count(c + 1);
        };

        SGB.SignalR.HubClient.requestSeen = function (id) {
            SGB.ViewModels.MyRequestsViewModel.setAsSeen(id);
        }
        
        SGB.SignalR.Connection.start();
    }
})(window, $);

//Tiles
(function (SGB) {
    SGB.ViewModels = SGB.ViewModels || {};
    SGB.ViewModels.TileViewModel = new vm();

    function vm() {
        var self = this;
        self.count = ko.observable(0);
        self.description = ko.observable("");
        self.waitingRequests = ko.observableArray([]);
        self.addNewRequest = function () {
            var previousCount = this.count();
            self.count(previousCount + 1);
        };
        self.details = ko.observableArray();
        self.seeAll = function () {
            SGB.Request.GetData("/Caller/GetWaitingRequests", function (data) {
                self.waitingRequests.removeAll();
                self.details.removeAll();
                self.description("");
                for (var i = 0; i < data.Result.length; i++) {
                    var request = data.Result[i];
                    var r = {
                        Id: request.ID,
                        Created: new Date(request.Created),
                        Checked: ko.observable(request.Checked),
                        Seen: request.Seen,
                        UserId: request.UserId
                    };
                    self.waitingRequests.push(r);
                }
            });
        };
        self.viewDetails = function () {
            var _this = this;
            _this.Checked(true);
            
            SGB.Request.GetData("Caller/GetRequestDetail?id=" + this.Id, function (data) {
                if (!_this.Clicked) {
                    var p = self.count();
                    self.count(p - 1);
                    _this.Clicked = true;
                }
                self.details.removeAll();
                for (var i = 0; i < data.Result.Details.length; i++) {
                    var d = data.Result.Details[i];
                    self.details.push(d);
                }
                self.description(data.Result.Notes);
            });
        }
        self.load = function () {
            SGB.Request.GetData("/Caller/GetRequestCount", function (data) {
                self.count(data.Result);
            });
        }
        self.init = function () {
            self.load();
        }
    };
})(SGB);

//Request
(function (SGB, w) {
    SGB.ViewModels = SGB.ViewModels || {};
    SGB.ViewModels.RequestViewModel = new vm();

    function vm() {
        var self = this;
        self.count = ko.observable(0);
        self.selected = ko.observable();
        self.selected.Options = ko.observableArray([]);
        self.tiles = ko.observableArray([]);
        self.requests = ko.observableArray([]);
        self.notes = ko.observable("");
        self.addNewRequest = function () {
            var obj = this;
            if (!obj.Options || obj.Options.length == 0) {
                var match = ko.utils.arrayFirst(self.requests(), function (c) {
                    return c.ID === obj.ID;
                });
                if (match) {
                    var p = match.Count();
                    if (p < 12)
                        match.Count(p + 1);
                    else {
                        SGB.Notify.Warning("Bildirim Sistemi", "Daha fazla istek yollayabileceğinizi düşünemedik");
                        return;
                    }
                }
                else {
                    var newItem = {};
                    newItem.ID = obj.ID;
                    newItem.Remove = function () {
                        self.requests.remove(this);
                    };
                    newItem.Name = obj.Name;
                    newItem.Count = ko.observable(1);
                    self.requests.push(newItem);
                }
                return;
            }

            metroDialog.open("#dialog");
            $("#dialog").css("top", "30px");
            self.selected.Options.removeAll();
            for (var i = 0; i < obj.Options.length; i++) {
                var item = {
                    ID: obj.ID,
                    Name: ko.observable(obj.Name + " (" + obj.Options[i] + ")"),
                    Count: ko.observable(0),
                    Description: ko.observable(""),
                    Plus: function () {
                        var p = this.Count();
                        if (p < 12)
                            this.Count(p + 1);
                        else {
                            SGB.Notify.Warning("Bildirim Sistemi", "Daha fazla istek yollayabileceğinizi düşünemedik");
                        }
                    },
                    Minus: function () {
                        var p = this.Count();
                        if (p > 0)
                            this.Count(p - 1);
                        else {
                            SGB.Notify.Error("Bildirim Sistemi", "Daha fazla eksiltme işlemi yapamazsınız. Listeden silmeyi deneyiniz");
                        }
                    }
                };
                self.selected.Options.push(item);
                self.selected.Save = function () {
                    for (var i = 0; i < self.selected.Options().length; i++) {
                        var data = self.selected.Options()[i];
                        if (data.Count() == 0)
                            continue;
                        var newItem = {};
                        newItem.ID = data.ID + "_" + i;
                        newItem.Remove = function () {
                            self.requests.remove(this);
                        };
                        newItem.Name = data.Name();
                        newItem.Count = ko.observable(data.Count());
                        var match = ko.utils.arrayFirst(self.requests(), function (c) {
                            return c.ID === newItem.ID;
                        });
                        if (match) {
                            var p = match.Count();
                            match.Count(p + data.Count());
                        }
                        else {
                            self.requests.push(newItem);
                        }
                    }
                    metroDialog.close('#dialog')
                }
            }

        };
        self.load = function () {
            SGB.Request.GetData("/Caller/GetTypes", function (data) {
                var list = data.Result;
                for (var i = 0; i < list.length; i++) {
                    var item = list[i];
                    self.tiles.push(item);
                }
            });
        }
        self.SaveAll = function () {
            var data = {
                Notes: self.notes(),
                Details: []
            }
            for (var i = 0; i < self.requests().length; i++) {
                var req = self.requests()[i];
                data.Details.push({ Name: req.Name, Count: req.Count() });
            }
            SGB.Request.PostData("/Caller/AddNewRequest", data, function (d) {
                SGB.Notify.ShowApiResult(d);
                setTimeout(function () {
                    w.location.href = "/Home/MyRequests";
                }, 1000);
            });
        }

        self.init = function () {
            self.load();
        }
    };
})(SGB, window);

//Login
(function (SGB) {
    SGB.ViewModels = SGB.ViewModels || {};
    SGB.ViewModels.LoginViewModel = new vm();

    function vm() {
        var self = this;
        self.users = ko.observableArray();
        self.locations = ko.observableArray();
        self.load = function () {
            SGB.Request.GetData("/Caller/GetAllUsers", function (data) {
                for (var i = 0; i < data.Result.length; i++) {
                    var d = data.Result[i];
                    self.users.push(d);
                }
            });

            SGB.Request.GetData("/Caller/GetLocations", function (data) {
                for (var i = 0; i < data.Result.length; i++) {
                    var d = data.Result[i];
                    self.locations.push(d);
                }
            });
        }
        self.init = function () {
            self.load();
        }
    }
})(SGB);

//MyRequests
(function (SGB, w, $) {
    SGB.ViewModels = SGB.ViewModels || {};
    SGB.ViewModels.MyRequestsViewModel = new vm();

    function vm() {
        var self = this;
        self.requests = ko.observableArray();

        self.load = function () {
            SGB.Request.GetData("/Caller/GetUserRequests", function (data) {
                for (var i = 0; i < data.Result.length; i++) {
                    var d = data.Result[i];
                    var obj = {
                        ID : d.ID,
                        Created: d.Created,
                        Seen: ko.observable(d.Seen),
                        Details : d.Details
                    }
                    self.requests.push(obj);
                }
                $("time.timeago").timeago();
            });
        }
        self.setAsSeen = function (id) {
            var match = ko.utils.arrayFirst(self.requests(), function (c) {
                return c.ID === id;
            });
            if (match)
                match.Seen(true);
        }
        self.init = function () {
            self.load();
        }
    }
})(SGB, window, $);
ko.bindingHandlers.dateString = {
    update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
        var value = valueAccessor(),
            allBindings = allBindingsAccessor();
        var valueUnwrapped = ko.utils.unwrapObservable(value);
        var pattern = allBindings.datePattern || 'dd-mm-yyyy';
        if (valueUnwrapped == undefined || valueUnwrapped == null) {
            $(element).text("");
        }
        else {
            var date = moment(valueUnwrapped, "DD-MM-YYYY HH:mm:ss"); 
            $(element).text(moment(date).format(pattern));
        }
    }
}
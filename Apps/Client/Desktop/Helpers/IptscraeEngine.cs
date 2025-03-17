using Lib.Common.Desktop.Interfaces;
using Lib.Core.Entities.Shared.Types;
using Mod.Scripting.Iptscrae.Entities;
using Mod.Scripting.Iptscrae.Enums;
using ThePalace.Client.Desktop.Interfaces;

namespace ThePalace.Client.Desktop.Helpers;

public class IptscraeEngine : Mod.Scripting.Iptscrae.Helpers.IptscraeEngine
{
    static IptscraeEngine()
    {
        #region Start Paint Commands

        RegisterCommand(
            new KeyValuePair<string, IptCommandFnc>(
                "LINE", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register4 = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.model.Screen.paintPenPos = {
                    //    v: register3.Value,
                    //    h: register4.Value,
                    //};

                    //$scope.encodeDrawCmd({
                    //    v: register3.Value,
                    //    h: register4.Value,
                    //}, [{
                    //    v: register1.Value - register3.Value,
                    //    h: register2.Value - register4.Value,
                    //}]);
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "LINETO", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.encodeDrawCmd({
                    //    v: $scope.model.Screen.paintPenPos.v,
                    //    h: $scope.model.Screen.paintPenPos.h,
                    //}, [{
                    //    v: register1.Value - $scope.model.Screen.paintPenPos.v,
                    //    h: register2.Value - $scope.model.Screen.paintPenPos.h,
                    //}]);

                    //$scope.model.Screen.paintPenPos = {
                    //    v: register1.Value,
                    //    h: register2.Value,
                    //};
                }),
            // Start Paint Commands
            new KeyValuePair<string, IptCommandFnc>(
                "PENPOS", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.model.Screen.paintPenPos = {
                    //    v: register1.Value,
                    //    h: register2.Value,
                    //};
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "PENTO", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //if (!$scope.model.Screen.paintPenPos) {
                    //    $scope.model.Screen.paintPenPos = {
                    //        v: 0,
                    //        h: 0,
                    //    };
                    //}

                    //var xCoord = $scope.model.Screen.paintPenPos.h + register2.Value;
                    //var yCoord = $scope.model.Screen.paintPenPos.v + register1.Value;

                    //$scope.model.Screen.paintPenPos = {
                    //    v: yCoord,
                    //    h: xCoord,
                    //};
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "PENCOLOR", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.model.Screen.paintPenColor = {
                    //    r: register3.Value % 256,
                    //    g: register2.Value % 256,
                    //    b: register1.Value % 256,
                    //};
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "PAINTCLEAR", (iptTracking, recursionDepth) =>
                {
                    //$scope.serverSend(
                    //    'MSG_DRAW',
                    //    {
                    //        Type = 'DC_Detonate',
                    //        layer: false,
                    //        data: null,
                    //    });
                }
            ),
            new KeyValuePair<string, IptCommandFnc>(
                "PAINTUNDO", (iptTracking, recursionDepth) =>
                {
                    //$scope.serverSend(
                    //    'MSG_DRAW',
                    //    {
                    //        Type = 'DC_Delete',
                    //        layer: false,
                    //        data: null,
                    //    });
                }
            ),
            new KeyValuePair<string, IptCommandFnc>(
                "PENBACK", (iptTracking, recursionDepth) =>
                {
                    //$scope.model.Screen.paintLayer = false;
                }
            ),
            new KeyValuePair<string, IptCommandFnc>(
                "PENFRONT", (iptTracking, recursionDepth) =>
                {
                    //$scope.model.Screen.paintLayer = true;
                }
            ),
            new KeyValuePair<string, IptCommandFnc>(
                "PENSIZE", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if ((int)register.Value < 1) register.Value = 1;

                    //$scope.model.Screen.paintPenSize = register.Value;
                }
            )
        );

        #endregion End Paint Commands

        #region Start Prop Commands

        RegisterCommand(
            new KeyValuePair<string, IptCommandFnc>(
                "MACRO", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    // TODO:
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "USERPROP", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //            if (register.Value < $scope.model.RoomInfo.UserList[j].propSpec.length) {
                    //                var propID = $scope.model.RoomInfo.UserList[j].propSpec[register.Value].id;
                    //
                    //                iptTracking.Stack.Push(new IptValue {
                    //                    Type = IptTypes.Integer,
                    //                    Value = propID,
                    //                });
                    //            }
                    //        }
                    //
                    //        break;
                    //    }
                    //}
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "DROPPROP", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //            var lastIndex = $scope.model.RoomInfo.UserList[j].propSpec.length - 1;
                    //            var propID = $scope.model.RoomInfo.UserList[j].propSpec[lastIndex].id;
                    //
                    //            $scope.serverSend(
                    //                'MSG_PROPNEW',
                    //
                    //                {
                    //                    propSpec: {
                    //                        id: propID,
                    //			            crc: 0,
                    //		            ),
                    //		            loc: {
                    //                        h: register2.Value,
                    //			            v: register1.Value,
                    //		            ),
                    //	            });
                    //
                    //            if (lastIndex == 0) {
                    //	            $scope.setProps(null);
                    //            }
                    //            else {
                    //	            $scope.model.RoomInfo.UserList[j].propSpec.splice(lastIndex, 1);
                    //
                    //	            $scope.setProps($scope.model.RoomInfo.UserList[j].propSpec);
                    //            }
                    //        }
                    //
                    //        break;
                    //    }
                    //}
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "DOFFPROP", (iptTracking, recursionDepth) =>
                {
                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //            $scope.model.RoomInfo.UserList[j].propSpec.splice($scope.model.RoomInfo.UserList[j].propSpec.length - 1, 1);
                    //
                    //            $scope.setProps($scope.model.RoomInfo.UserList[j].propSpec.length > 0 ? $scope.model.RoomInfo.UserList[j].propSpec : null);
                    //        }
                    //
                    //        break;
                    //    }
                    //}
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "TOPPROP", (iptTracking, recursionDepth) =>
                {
                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        var propID = 0;
                    //
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //            propID = $scope.model.RoomInfo.UserList[j].propSpec[$scope.model.RoomInfo.UserList[j].propSpec.length - 1].id;
                    //        }
                    //
                    //        iptTracking.Stack.Push(new IptValue {
                    //            Type = IptTypes.Integer,
                    //            Value = propID,
                    //        });
                    //
                    //        break;
                    //    }
                    //}
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "HASPROP", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        var propID = 0;
                    //
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //            switch (register.Type) {
                    //                case IptTypes.String:
                    //		            for (var k in $scope.model.Screen.assetCache) {
                    //                        if ($scope.model.Screen.assetCache[k].name == register.Value) {
                    //                            propID = k;
                    //
                    //                            break;
                    //                        }
                    //                    }
                    //
                    //                    break;
                    //                case IptTypes.Integer:
                    //                    propID = register.Value;
                    //
                    //                    break;
                    //            }
                    //        }
                    //
                    //        iptTracking.Stack.Push(new IptValue {
                    //            Type = IptTypes.Bool,
                    //            Value = propID != 0 ? 1 : 0,
                    //        });
                    //
                    //        break;
                    //    }
                    //}
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "CLEARLOOSEPROPS", (iptTracking, recursionDepth) =>
                {
                    //$scope.serverSend(
                    //    'MSG_PROPDEL',
                    //    {
                    //        propNum: -1
                    //    });
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "ADDLOOSEPROP", (iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.String);

                    if (register1.Type != register2.Type && register1.Type != IptVariableTypes.Integer)
                        throw new Exception($"Wrong datatype {register1.Type}...");

                    switch (register3.Type)
                    {
                        case IptVariableTypes.String:
                            //for (var k in $scope.model.Screen.assetCache) {
                            //    if ($scope.model.Screen.assetCache[k].name == register3.Value) {
                            //        $scope.serverSend(
                            //            'MSG_PROPNEW',
                            //
                            //            {
                            //                propSpec: {
                            //                    id: k,
                            //		            crc: 0,
                            //	            ),
                            //	            loc: {
                            //                    h: register2.Value,
                            //		            v: register1.Value,
                            //	            ),
                            //            });
                            //
                            //        break;
                            //    }
                            //}

                            break;
                        case IptVariableTypes.Integer:
                            //$scope.serverSend(
                            //    'MSG_PROPNEW',
                            //
                            //    {
                            //        propSpec: {
                            //            id: register3.Value,
                            //            crc: 0,
                            //        ),
                            //        loc: {
                            //            h: register2.Value,
                            //            v: register1.Value,
                            //        ),
                            //    });

                            break;
                    }
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "REMOVEPROP", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //
                    //            var propID = 0;
                    //
                    //            switch (register.Type) {
                    //                case IptTypes.String:
                    //		            for (var k in $scope.model.Screen.assetCache) {
                    //                        if ($scope.model.Screen.assetCache[k].name == register.Value) {
                    //                            propID = k;
                    //
                    //                            break;
                    //                        }
                    //                    }
                    //
                    //                    break;
                    //                case IptTypes.Integer:
                    //                    propID = register.Value;
                    //
                    //                    break;
                    //                default: throw new Exception($"Wrong datatype {register.Type}...");
                    //
                    //                    break;
                    //            }
                    //
                    //            if (propID != 0) {
                    //                for (var k = 0; k < $scope.model.RoomInfo.UserList[j].propSpec.length; k++) {
                    //                    if ($scope.model.RoomInfo.UserList[j].propSpec[k].id == propID) {
                    //			            $scope.model.RoomInfo.UserList[j].propSpec.splice(k, 1);
                    //
                    //                        break;
                    //                    }
                    //                }
                    //            }
                    //        }
                    //
                    //        break;
                    //    }
                    //}

                    //$scope.setProps(propSpec);
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "DONPROP", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String, IptVariableTypes.Integer);
                    var propSpec = new List<AssetSpec>();

                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        if ($scope.model.RoomInfo.UserList[j].propSpec && $scope.model.RoomInfo.UserList[j].propSpec.length > 0) {
                    //            propSpec = $scope.model.RoomInfo.UserList[j].propSpec;
                    //        }
                    //
                    //        switch (register.Type) {
                    //            case IptTypes.String:
                    //	            for (var k in $scope.model.Screen.assetCache) {
                    //                    if ($scope.model.Screen.assetCache[k].name == register.Value) {
                    //                        propSpec.push({
                    //                            id: k,
                    //				            crc: 0,
                    //			            });
                    //
                    //                        break;
                    //                    }
                    //                }
                    //
                    //                break;
                    //            case IptTypes.Integer:
                    //                propSpec.push({
                    //                    id: register.Value,
                    //		            crc: 0,
                    //	            });
                    //
                    //                break;
                    //            default: throw new Exception($"Wrong datatype {register.Type}...");
                    //
                    //                break;
                    //        }
                    //
                    //        break;
                    //    }
                    //}

                    //$scope.setProps(propSpec);
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "SETPROPS", (iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Array);

                    var propSpec = new List<AssetSpec>();

                    //for (var j = 0; j < register.Value.length; j++) {
                    //    switch (register.Value[j].Type) {
                    //        case IptTypes.String:
                    //            for (var k in $scope.model.Screen.assetCache) {
                    //                if ($scope.model.Screen.assetCache[k].name == register.Value[j].Value) {
                    //                    propSpec.push({
                    //                        id: k,
                    //			            crc: 0,
                    //		            });
                    //
                    //                    break;
                    //                }
                    //            }
                    //
                    //            break;
                    //        case IptTypes.Integer:
                    //            propSpec.push({
                    //                id: register.Value[j].Value,
                    //	            crc: 0,
                    //            });
                    //
                    //            break;
                    //        default: throw new Exception($"Wrong datatype {register.Type}...");
                    //    }
                    //}

                    //$scope.setProps(propSpec.length ? propSpec : null);
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "SHOWLOOSEPROPS", (iptTracking, recursionDepth) =>
                {
                    //for (var j = 0; j < $scope.model.RoomInfo.LooseProps.length; j++) {
                    //    var prop = $scope.model.RoomInfo.LooseProps[j];
                    //
                    //    $scope.model.Interface.LogList.push({
                    //        userName: this.iptEngineUsername,
                    //        isWhisper: true,
                    //        text: ''.concat(prop.propSpec.id, ' ', prop.loc.h, ' ', prop.loc.v, ' ADDLOOSEPROP'),
                    //    })
                    //}
                }),
            new KeyValuePair<string, IptCommandFnc>(
                "NBRUSERPROPS", (iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IClientDesktopSessionState<IDesktopApp>>() is not IClientDesktopSessionState<IDesktopApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        sessionState.UserDesc?.UserRec?.PropSpec?.Length ?? 0));
                }));

        #endregion End Prop Commands
    }
}
using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using Lib.Common.Factories.Core;
using Lib.Common.Helpers;
using Lib.Core.Entities.Network.Client.Network;
using Lib.Core.Entities.Shared.Users;
using Lib.Core.Helpers.Network;
using Lib.Core.Interfaces.Core;
using Lib.Logging.Entities;
using Mod.Scripting.Iptscrae.Entities;
using Mod.Scripting.Iptscrae.Enums;

namespace Mod.Scripting.Iptscrae.Helpers;

using IptAtomList = List<IptVariable>;

public class IptscraeEngine
{
    protected const decimal CONST_IptVersion = (decimal)3.0;
    protected const int CONST_gNestedAtomlistMaxDepth = 256;
    protected const int CONST_gNestedArrayMaxDepth = 256;
    protected const int CONST_gWhileMaxIteration = 7500;
    protected const int CONST_gMaxPaintPenSize = 20;
    protected const int CONST_gStackMaxSize = 1024;
    protected const string CONST_IptEngineName = "IptscraeEngine";
    protected const string CONST_ClientType = "IptscraeEngine";

    protected static readonly IReadOnlyDictionary<string, IptEventTypes> EventTypes =
        Enum.GetValues<IptEventTypes>()
            .ToDictionary(v => v.ToString().ToUpperInvariant(), v => v);

    protected static readonly ConcurrentDictionary<string, object> IptCommands =
        new(new Dictionary<string, object>
        {
            #region Iptscrae Commands

            {
                "IPTVERSION", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)CONST_IptVersion));
                })
            },

            #region Iptscrae Version 1

            #region Start Aliases

            { "ROOMGOTO", "GOTOROOM" },
            { "CLEARPROPS", "NAKED" },
            { "NETGOTO", "GOTOURL" },
            { "USERID", "WHOME" },
            { "CHAT", "SAY" },
            { "ID", "ME" },

            #endregion End Aliases

            #region Start Sound Commands

            {
                "SOUND", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //if ($scope.model.Interface.soundsEnabled) {
                    //    var audioUrl = $scope.model.ServerInfo.mediaUrl + ($scope.model.ServerInfo.mediaUrl.substring($scope.model.ServerInfo.mediaUrl.length - 1, 1) == '/' ? '' : '/') + register.Value;
                    //
                    //    $scope.model.Application.soundPlayer.preload({
                    //        sourceUrl: audioUrl,
                    //        resolve: function (response) {
                    //            this.play();
                    //        },
                    //        reject: function (errors) {
                    //        },
                    //    });
                    //    $scope.model.Application.soundPlayer.load();
                    //}
                })
            },
            {
                "SOUNDPAUSE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    //$scope.model.Application.soundPlayer.pause();
                })
            },
            {
                "MIDIPLAY", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //if ($scope.model.Interface.soundsEnabled) {
                    //    var audioUrl = $scope.model.ServerInfo.mediaUrl + ($scope.model.ServerInfo.mediaUrl.substring($scope.model.ServerInfo.mediaUrl.length - 1, 1) == '/' ? '' : '/') + register.Value;
                    //
                    //    $scope.model.Application.midiPlayer.play(audioUrl);
                    //}
                })
            },
            {
                "MIDISTOP", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    //$scope.model.Application.midiPlayer.stop();
                })
            },

            #endregion End Sound Commands

            #region Start Math Commands

            {
                "SINE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)Math.Sin((int)register.Value) * 1000));
                })
            },
            {
                "COSINE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)Math.Cos((int)register.Value) * 1000));
                })
            },
            {
                "TANGENT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)Math.Tan((int)register.Value) * 1000));
                })
            },
            {
                "SQUAREROOT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)Math.Sqrt((int)register.Value) * 1000));
                })
            },
            {
                "RANDOM", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        RndGenerator.Next((int)register.Value)));
                })
            },
            {
                "MOD", (IptCommandFnc)((iptTracking, recursionDepth) => { Operator(iptTracking, "%", recursionDepth); })
            },

            #endregion End Math Commands

            #region Start Time Commands

            {
                "TICKS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        DateTime.UtcNow.ToTicks()));
                })
            },
            {
                "DATETIME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        DateTime.UtcNow.ToTimestamp()));
                })
            },

            #endregion End Time Commands

            #region Start Stack Commands

            {
                "OVER", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (iptTracking.Stack.Count < 2) throw new Exception("Not enough items on the stack...");

                    iptTracking.Stack.Push(iptTracking.Stack[iptTracking.Stack.Count - 2]);
                })
            },
            {
                "PICK", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (iptTracking.Stack.Count <= (int)register.Value)
                        throw new Exception("Not enough items on the stack...");

                    iptTracking.Stack.Push(iptTracking.Stack[iptTracking.Stack.Count - (int)register.Value - 1]);
                })
            },
            {
                "DUP", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = iptTracking.Stack.PeekL();
                    iptTracking.Stack.Push(register);
                })
            },
            { "POP", (IptCommandFnc)((iptTracking, recursionDepth) => { popStack(iptTracking); }) },
            {
                "SWAP", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = popStack(iptTracking);
                    var register2 = popStack(iptTracking);

                    iptTracking.Stack.Push(register1);
                    iptTracking.Stack.Push(register2);
                })
            },
            {
                "STACKDEPTH", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        iptTracking.Stack.Count));
                })
            },

            #endregion End Stack Commands

            #region Start Message Commands

            {
                "ROOMMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.serverSend(
                    //    'MSG_RMSG',
                    //    {
                    //        text: register.Value,
                    //    });
                })
            },
            {
                "SUSRMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.serverSend(
                    //    'MSG_SMSG',
                    //    {
                    //        text: register.Value,
                    //    });
                })
            },
            {
                "GLOBALMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.serverSend(
                    //    'MSG_GMSG',
                    //    {
                    //        text: register.Value,
                    //    });
                })
            },
            {
                "LOCALMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$.connection.proxyHub.client.receive(
                    //    'MSG_TALK',
                    //    $scope.model.UserInfo.userId,
                    //    {
                    //        text: register.Value,
                    //        localmsg: true,
                    //    });
                })
            },
            {
                "STATUSMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.setStatusMsg(register.Value);
                })
            },
            {
                "PRIVATEMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.serverSend(
                    //    'MSG_XWHISPER',
                    //    {
                    //        target: register1.Value,
                    //        text: register2.Value,
                    //    });
                })
            },
            {
                "LOGMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.model.Interface.LogList.push({
                    //    userName: this.iptEngineUsername,
                    //    text: register.Value,
                    //    isWhisper: true,
                    //});
                })
            },
            {
                "SAY", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    EventType = EventTypes.MSG_XTALK,
                    //    protocolSend = new MSG_XTALK
                    //    {
                    //        Text = register.Value.ToString(),
                    //    },
                    //});
                })
            },
            {
                "SAYAT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.String);

                    //$scope.serverSend(
                    //    'MSG_XTALK',
                    //    {
                    //        text: ''.concat('@', register2.Value, ',', register1.Value, ' ', register3.Value),
                    //    });
                })
            },

            #endregion End Message Commands

            #region Start String Commands

            {
                "LOWERCASE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    var value = register.Value.ToString().ToLowerInvariant();

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        value));
                })
            },
            {
                "UPPERCASE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);
                    register = getVariable(iptTracking, register);

                    var value = register.Value.ToString().ToUpperInvariant();

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        value));
                })
            },
            {
                "STRINDEX", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.String);
                    var register2 = getStack(iptTracking, IptVariableTypes.String);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        register2.Value.ToString().IndexOf(register1.Value.ToString())));
                })
            },
            {
                "STRLEN", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        register.Value.ToString().Length));
                })
            },
            {
                "SUBSTR", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.String);
                    var register2 = getStack(iptTracking, IptVariableTypes.String);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        register2.Value.ToString().IndexOf(register1.Value.ToString()) != -1 ? 1 : 0));
                })
            },
            {
                "SUBSTRING", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.String);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        register3.Value.ToString().Substring((int)register2.Value, (int)register1.Value)));
                })
            },
            {
                "GREPSTR", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.String);
                    var register2 = getStack(iptTracking, IptVariableTypes.String);

                    var regExp = new Regex(register1.Value.ToString());

                    iptTracking.Grep = regExp.Matches(register2.Value.ToString())
                        .ToArray();

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        iptTracking.Grep != null ? 1 : 0));
                })
            },
            {
                "GREPSUB", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    if (iptTracking.Grep == null) return;

                    register.Value = (register.Value?.ToString() ?? string.Empty).Trim();

                    for (var j = 1; j < 10; j++)
                        register.Value = register.Value?.ToString()
                            ?.Replace($"${j}", iptTracking.Grep[j].Value);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        register.Value));
                })
            },
            {
                "STRTOATOM", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Atomlist,
                        Parse(iptTracking, register.Value.ToString(), false)));
                })
            },

            #endregion End String Commands

            #region Start Boolean Commands

            {
                "TRUE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        1));
                })
            },
            {
                "FALSE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        0));
                })
            },
            {
                "NOT", (IptCommandFnc)((iptTracking, recursionDepth) => { Operator(iptTracking, "!", recursionDepth); })
            },
            {
                "OR", (IptCommandFnc)((iptTracking, recursionDepth) => { Operator(iptTracking, "||", recursionDepth); })
            },
            {
                "AND",
                (IptCommandFnc)((iptTracking, recursionDepth) => { Operator(iptTracking, "&&", recursionDepth); })
            },
            {
                "IF", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist);

                    if ((int)register1.Value != 0)
                        Executor(register2.Value as IptAtomList, iptTracking, recursionDepth + 1);
                })
            },
            {
                "IFELSE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist);
                    var register3 = getStack(iptTracking, IptVariableTypes.Atomlist);

                    if ((int)register1.Value != 0)
                        Executor(register3.Value as IptAtomList, iptTracking, recursionDepth + 1);
                    else
                        Executor(register2.Value as IptAtomList, iptTracking, recursionDepth + 1);
                })
            },

            #endregion End Boolean Commands

            #region Start Array Commands

            {
                "ARRAY", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Array,
                        new IptAtomList((int)register.Value)));
                })
            },
            {
                "DEF", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = popStack(iptTracking, IptVariableTypes.Variable);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist, IptVariableTypes.Array);

                    setVariable(iptTracking, register1, register2, recursionDepth);
                })
            },
            {
                "GET", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Array);

                    var array = register2.Value as IptAtomList;

                    if ((int)register1.Value >= array.Count)
                        throw new Exception($"Index {register1.Value} out of bounds...");

                    iptTracking.Stack.Push(array[(int)register1.Value]);
                })
            },
            {
                "PUT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Array);
                    var register3 = getStack(iptTracking);

                    var array = register2.Value as IptAtomList;

                    if ((int)register1.Value >= array.Count)
                        throw new Exception($"Index {register1.Value} out of bounds...");

                    array[(int)register1.Value] = register3;
                })
            },
            {
                "LENGTH", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String, IptVariableTypes.Array);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (register.Value as IptAtomList).Count));
                })
            },

            #endregion End Array Commands

            #region Start Variable Commands

            {
                "ITOA", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        register.Value.ToString()));
                })
            },
            {
                "ATOI", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        int.Parse(register.Value.ToString())));
                })
            },
            {
                "TOPTYPE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = iptTracking.Stack.PeekL();

                    var typeID = register.Type switch
                    {
                        IptVariableTypes.Bool or
                            IptVariableTypes.Integer => 1,
                        IptVariableTypes.Variable => 2,
                        IptVariableTypes.Atomlist => 3,
                        IptVariableTypes.String => 4,
                        IptVariableTypes.Array => 6,
                        _ => 0
                    };

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        typeID));
                })
            },
            {
                "VARTYPE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = iptTracking.Stack.PeekL();
                    register = getVariable(iptTracking, register);

                    var typeID = register.Type switch
                    {
                        IptVariableTypes.Bool or
                            IptVariableTypes.Integer => 1,
                        IptVariableTypes.Variable => 2,
                        IptVariableTypes.Atomlist => 3,
                        IptVariableTypes.String => 4,
                        IptVariableTypes.Array => 6,
                        _ => 0
                    };

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        typeID));
                })
            },

            #endregion End Variable Commands

            #region Start Spot Commands

            {
                "INSPOT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.UserList.length; j++) {
                    //    if ($scope.model.RoomInfo.UserList[j].userID == $scope.model.UserInfo.userId) {
                    //        var xCoord = $scope.model.RoomInfo.UserList[j].roomPos.h;
                    //        var yCoord = $scope.model.RoomInfo.UserList[j].roomPos.v;
                    //        var inside = false;
                    //
                    //        for (var k = 0; k < !inside && $scope.model.RoomInfo.SpotList.length; k++) {
                    //            var spot = $scope.model.RoomInfo.SpotList[k];
                    //
                    //            if (spot.id == register.Value) {
                    //	            var polygon = [];
                    //
                    //	            for (var l = 0; l < spot.Vortexes.length; l++) {
                    //		            polygon.push({
                    //			            v: spot.loc.v + spot.Vortexes[l].v,
                    //			            h: spot.loc.h + spot.Vortexes[l].h,
                    //		            });
                    //	            }
                    //
                    //	            inside = utilService.pointInPolygon(polygon, {
                    //		            v: yCoord,
                    //		            h: xCoord,
                    //	            });
                    //
                    //	            break;
                    //            }
                    //        }
                    //
                    //        iptTracking.Stack.Push(new IptValue {
                    //            Type = IptTypes.Bool,
                    //            Value = inside ? 1 : 0,
                    //        });
                    //
                    //        break;
                    //    }
                    //}
                })
            },
            {
                "LOCK", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.serverSend(
                    //    'MSG_DOORLOCK',
                    //    {
                    //        roomID: $scope.model.RoomInfo.roomId,
                    //        spotID: register.Value,
                    //    });
                })
            },
            {
                "UNLOCK", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.serverSend(
                    //    'MSG_DOORUNLOCK',
                    //    {
                    //        roomID: $scope.model.RoomInfo.roomId,
                    //        spotID: register.Value,
                    //    });
                })
            },
            {
                "ISLOCKED", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.SpotList.length; j++) {
                    //    var spot = $scope.model.RoomInfo.SpotList[j];
                    //    if (spot.id == register.Value) {
                    //        iptTracking.Stack.Push(new IptValue {
                    //            Type = IptTypes.Bool,
                    //            Value = spot.Type == HotSpotTypes.HT_Door && spot.state != 0 ? 1 : 0,
                    //        });
                    //
                    //        break;
                    //    }
                    //}
                })
            },
            {
                "GETSPOTSTATE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.SpotList.length; j++) {
                    //    if ($scope.model.RoomInfo.SpotList[j].id == register.Value) {
                    //        iptTracking.Stack.Push(new IptValue {
                    //            Type = IptTypes.Integer,
                    //            Value = $scope.model.RoomInfo.SpotList[j].state,
                    //        });
                    //
                    //        break;
                    //    }
                    //}
                })
            },
            {
                "SETSPOTSTATE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.SpotList.length; j++) {
                    //    if ($scope.model.RoomInfo.SpotList[j].id == register1.Value) {
                    //        $scope.serverSend(
                    //            'MSG_SPOTSTATE',
                    //            {
                    //	            roomID: $scope.model.RoomInfo.roomId,
                    //	            spotID: register1.Value,
                    //	            state: register2.Value,
                    //            });
                    //
                    //        break;
                    //    }
                    //}
                })
            },
            {
                "SETSPOTSTATELOCAL", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //for (var j = 0; j < $scope.model.RoomInfo.SpotList.length; j++) {
                    //    if ($scope.model.RoomInfo.SpotList[j].id == register1.Value) {
                    //        $scope.model.RoomInfo.SpotList[j].state == register2.Value;
                    //
                    //        $scope.model.Screen.spotLayerUpdate = true;
                    //
                    //        $scope.Screen_OnDraw();
                    //
                    //        break;
                    //    }
                    //}
                })
            },
            {
                "SETLOC", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.serverSend(
                    //    'MSG_SPOTMOVE',
                    //    {
                    //        roomID: $scope.model.RoomInfo.roomId,
                    //        spotID: register1.Value,
                    //        pos: {
                    //            h: register3.Value,
                    //            v: register2.Value,
                    //        }
                    //
                    //    });
                })
            },
            {
                "SETPICLOC", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register3 = getStack(iptTracking, IptVariableTypes.Integer);

                    //$scope.serverSend(
                    //    'MSG_PICTMOVE',
                    //    {
                    //        roomID: $scope.model.RoomInfo.roomId,
                    //        spotID: register1.Value,
                    //        pos: {
                    //            h: register3.Value,
                    //            v: register2.Value,
                    //        }
                    //
                    //    });
                })
            },
            {
                "SELECT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //$timeout(
                    //    $scope.Spot_OnEvent,
                    //    1,
                    //    false,
                    //    register.Value,
                    //    'SELECT'
                    //);
                })
            },
            {
                "SETALARM", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    //var futureTicks = Math.floor((register2.Value / 6) * 100);
                    //$timeout(
                    //    $scope.Spot_OnEvent,
                    //    futureTicks,
                    //    false,
                    //    register1.Value,
                    //    'ALARM'
                    //);
                })
            },
            {
                "ME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var index = iptTracking.Variables.TryGetValue("ME", out var variable)
                        ? (int)variable.Variable.Value
                        : 0;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        index));
                })
            },
            {
                "DEST", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    var index = iptTracking.Variables.TryGetValue("ME", out var variable)
                        ? (int)variable.Variable.Value
                        : 0;
                    if (index < 0) throw new Exception("Index out of bounds...");

                    //var spots = sessionState.RoomInfo.HotSpots.ToArray();
                    //if (index >= spots.Length) throw new Exception("Index out of bounds...");

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //spots[index].Dest,
                        0));
                })
            },
            {
                "DOORIDX", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    //var doorList = [];

                    //for (var j = 0; j < $scope.model.RoomInfo.SpotList.length; j++) {
                    //    if ($scope.model.RoomInfo.SpotList[j].Type == HotSpotTypes.HT_Door) {
                    //        doorList.push($scope.model.RoomInfo.SpotList[j]);
                    //    }
                    //}

                    //if (register.Value < doorList.length) {
                    //    iptTracking.Stack.Push(new IptValue {
                    //        Type = IptTypes.Integer,
                    //        Value = doorList[register.Value].id,
                    //    });
                    //
                    //    break;
                    //}
                    //else {
                    //    throw 'Index out of bounds...';
                    //}
                })
            },
            {
                "SPOTDEST", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    var index = (int)register.Value;
                    if (index < 0) throw new Exception("Index out of bounds...");

                    //var spots = sessionState.RoomInfo.HotSpots.ToArray();
                    //if (index >= spots.Length) throw new Exception("Index out of bounds...");

                    iptTracking.Stack.Push(new IptVariable(IptVariableTypes.Integer));
                    //spots[index].Dest,
                })
            },
            {
                "NBRSPOTS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(IptVariableTypes.Integer));
                    //sessionState.RoomInfo.HotSpots.Count,
                })
            },
            {
                "NBRDOORS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(IptVariableTypes.Integer));
                    //sessionState.RoomInfo.HotSpots
                    //    .Where(s => ((HotspotTypes)s.Flags & HotspotTypes.HS_Door) == HotspotTypes.HS_Door)
                    //    .Count(),
                })
            },
            {
                "SPOTNAME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    var index = (int)register.Value;
                    if (index < 0) throw new Exception("Index out of bounds...");

                    //var spots = sessionState.RoomInfo.HotSpots.ToArray();
                    //if (index >= spots.Length) throw new Exception("Index out of bounds...");

                    iptTracking.Stack.Push(new IptVariable(IptVariableTypes.String /*, spots[index].Name */));
                })
            },

            #endregion End Spot Commands

            #region Start Functional Commands

            {
                "BREAK",
                (IptCommandFnc)((iptTracking, recursionDepth) => { iptTracking.Flags |= IptTrackingFlags.Break; })
            },
            {
                "RETURN",
                (IptCommandFnc)((iptTracking, recursionDepth) => { iptTracking.Flags |= IptTrackingFlags.Return; })
            },
            { "EXIT", (IptCommandFnc)((iptTracking, recursionDepth) => { throw new Exception("Exiting Script"); }) },
            {
                "EXEC", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Atomlist);

                    Executor(register.Value as IptAtomList, iptTracking, recursionDepth + 1);
                })
            },
            {
                "ALARMEXEC", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist);

                    iptTracking.Alarms.Add(new IptAlarm(register2.Value as IptAtomList, (int)register1.Value));
                })
            },
            {
                "WHILE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Atomlist);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist);

                    var limit = CONST_gWhileMaxIteration;

                    iptTracking.Flags &= ~IptTrackingFlags.Break;

                    while ((iptTracking.Flags & IptTrackingFlags.Break) == 0 && limit-- > 0)
                    {
                        Executor(register1.Value as IptAtomList, iptTracking, recursionDepth + 1);

                        var register3 = popStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                        if ((int)register3.Value == 0) break;

                        Executor(register2.Value as IptAtomList, iptTracking, recursionDepth + 1);
                    }

                    if (limit < 1) throw new Exception("Endless loop, breaking...");
                })
            },
            {
                "FOREACH", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Array);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist);

                    var atom = register1.Value as IptAtomList;

                    iptTracking.Flags &= ~IptTrackingFlags.Break;

                    for (var j = 0; (iptTracking.Flags & IptTrackingFlags.Break) == 0 && j < atom.Count; j++)
                    {
                        iptTracking.Stack.Push(atom[j]);

                        Executor(register2.Value as IptAtomList, iptTracking, recursionDepth + 1);
                    }
                })
            },
            {
                "GLOBAL", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = popStack(iptTracking);
                    if (register.Type != IptVariableTypes.Variable)
                        throw new Exception($"Wrong datatype {register.Type}...");

                    var key = register.Value?.ToString();

                    if (!iptTracking.Variables.TryGetValue(key, out var value))
                    {
                        iptTracking.Variables[key] = new IptMetaVariable
                        {
                            Depth = recursionDepth,
                            Flags = IptMetaVariableFlags.IsGlobal,
                            Variable = new IptVariable(IptVariableTypes.Integer, 0),
                        };
                    }
                    else
                    {
                        var flags = value.Flags;

                        if ((flags & IptMetaVariableFlags.IsReadOnly) == IptMetaVariableFlags.IsReadOnly ||
                            (flags & IptMetaVariableFlags.IsSpecial) == IptMetaVariableFlags.IsSpecial) return;

                        value.Flags |= IptMetaVariableFlags.IsGlobal;
                    }
                })
            },

            #endregion End Functional Commands

            #region Start User Commands

            {
                "WHOME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //sessionState.UserID,
                        0));
                })
            },
            {
                "MOVE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    eventType = EventTypes.MSG_USERMOVE,
                    //    protocolSend = new MSG_USERMOVE
                    //    {
                    //        Pos = new Point
                    //        {
                    //            X = (short)((int)register1.Value + sessionState.UserInfo.roomPos.h),
                    //            Y = (short)((int)register2.Value + sessionState.UserInfo.roomPos.v),
                    //        },
                    //    },
                    //});
                })
            },
            {
                "SETPOS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    eventType = Core.Enums.EventTypes.MSG_USERMOVE,
                    //    protocolSend = new MSG_USERMOVE
                    //    {
                    //        pos = new Point
                    //        {
                    //            h = (short)(int)register1.Value,
                    //            v = (short)(int)register2.Value,
                    //        },
                    //    },
                    //});
                })
            },
            {
                "MOUSEPOS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    //iptTracking.Stack.Push(new IptValue {
                    // Type = IptTypes.Integer,
                    // Value = $window.MousePositionX,
                    //});

                    //iptTracking.Stack.Push(new IptValue {
                    // Type = IptTypes.Integer,
                    // Value = $window.MousePositionY,
                    //});
                })
            },
            {
                "POSX", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //sessionState.UserDesc?.UserInfo?.RoomPos?.HAxis ?? 0,
                        0));
                })
            },
            {
                "POSY", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //sessionState.UserDesc?.UserInfo?.RoomPos?.VAxis ?? 0,
                        0));
                })
            },
            {
                "SERVERNAME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        //sessionState.ServerName,
                        0));
                })
            },
            {
                "ROOMNAME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        //sessionState.RoomName,
                        0));
                })
            },
            {
                "ROOMID", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //sessionState.RoomID,
                        0));
                })
            },
            {
                "USERNAME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        //sessionState.Name,
                        0));
                })
            },
            {
                "NBRROOMUSERS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //sessionState.RoomUsers.Count,
                        0));
                })
            },
            {
                "ISWIZARD", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        //(sessionState.UserFlags & UserFlags.U_Moderator) != 0 ? 1 : 0,
                        0));
                })
            },
            {
                "ISGOD", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        //(sessionState.UserFlags & UserFlags.U_Administrator) != 0 ? 1 : 0,
                        0));
                })
            },
            {
                "ISGUEST", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Bool,
                        //(sessionState.UserFlags & UserFlags.U_Guest) != 0 ? 1 : 0,
                        0));
                })
            },
            {
                "NAKED", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //if (sessionState.UserInfo.assetSpec != null)
                    //    AssetsManager.Current.FreeAssets(
                    //        false,
                    //        sessionState.UserInfo.assetSpec
                    //            .Select(p => p.id)
                    //            .ToArray());
                    //sessionState.UserInfo.assetSpec.Clear();

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    eventType = Core.Enums.EventTypes.MSG_USERPROP,
                    //    protocolSend = new MSG_USERPROP
                    //    {
                    //        assetSpec = new(),
                    //    },
                    //});

                    throw new NotImplementedException("NAKED");
                })
            },
            {
                "WHOTARGET", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //iptTracking.Stack.Push(new IptVariable
                    //{
                    //    Type = IptVariableTypes.Integer,
                    //    Value = 0,
                    //});

                    throw new NotImplementedException("WHOTARGET");
                })
            },
            {
                "SETFACE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    eventType = Core.Enums.EventTypes.MSG_USERFACE,
                    //    protocolSend = new MSG_USERFACE
                    //    {
                    //        faceNbr = (short)(int)register.Value,
                    //    },
                    //});

                    throw new NotImplementedException("SETFACE");
                })
            },
            {
                "SETCOLOR", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Bool, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    EventType = EventTypes.MSG_USERCOLOR,
                    //    protocolSend = new MSG_USERCOLOR
                    //    {
                    //        colorNbr = (short)(int)register.Value,
                    //    },
                    //});

                    throw new NotImplementedException("SETCOLOR");
                })
            },
            {
                "WHONAME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //var sourceID = (UInt32)register.Value;

                    //var user = (UserRec?)null;
                    //if (sourceID > 0)
                    //      user = sessionState.RoomUsers.GetValueLocked(sourceID);

                    //if (user == null) return;

                    //iptTracking.Stack.Push(new IptVariable
                    //{
                    //    Type = IptVariableTypes.String,
                    //    Value = user.name,
                    //});

                    throw new NotImplementedException("WHONAME");
                })
            },
            { "WHOCHAT", (IptCommandFnc)((iptTracking, recursionDepth) => throw new NotImplementedException("WHOCHAT")) },
            {
                "WHOPOS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    var user = (UserRec?)null;

                    switch (register.Type)
                    {
                        case IptVariableTypes.String:
                        {
                            var key = register.Value?.ToString()?.Trim();
                            //if (!string.IsNullOrWhiteSpace(key))
                            //    user = sessionState.RoomUsers.Values
                            //        .Where(u => u.Name == key)
                            //        .FirstOrDefault();
                        }

                            break;
                        case IptVariableTypes.Integer:
                        {
                            var sourceID = (int)register.Value;
                            //if (sourceID > 0)
                            //    user = sessionState.RoomUsers.GetValueLocked(sourceID);
                        }

                            break;
                        default: throw new Exception($"Wrong datatype {register.Type}...");
                    }

                    if (user == null) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        user.RoomPos.HAxis));

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        user.RoomPos.VAxis));
                })
            },
            {
                "ROOMUSER", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    var index = (int)register.Value;
                    if (index < 0) throw new Exception("Index out of bounds...");

                    //var users = sessionState.RoomUsers.Values.ToArray();
                    //if (index >= users.Length) throw new Exception("Index out of bounds...");

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //users[index].userID,
                        0));
                })
            },
            {
                "GOTOROOM", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    sessionState.Send(
                        0,
                        new MSG_ROOMGOTO
                        {
                            Dest = (short)register.Value,
                        });
                })
            },

            #endregion End User Commands

            #region Start Misc Commands

            {
                "LAUNCHAPP", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = popStack(iptTracking, IptVariableTypes.String);

                    // Effectively does nothing, just for legacy support

                    throw new NotImplementedException("LAUNCHAPP");
                })
            },
            {
                "DIMROOM", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    var value = new Switch<float>(1F - (int)register.Value / 100F)
                        .Case(v => v > 1F, v => 1F)
                        .Case(v => v < 0, v => 0F)
                        .Execute();

                    //sessionState.LayerOpacity(value, ScreenLayers.DimRoom);
                    //sessionState.RefreshScreen(ScreenLayers.DimRoom);

                    throw new NotImplementedException("DIMROOM");
                })
            },
            {
                "DELAY", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    Thread.Sleep(IptAlarm.TicksToMs<int>(register.Value));
                })
            },
            {
                "BEEP", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Audio, null, sessionState, AudioCommandTypes.BEEP);

                    throw new NotImplementedException("BEEP");
                })
            },
            {
                "CLIENTTYPE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.String,
                        CONST_ClientType));
                })
            },
            {
                "GOTOURL", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    //$window.open(register.Value, '_blank');

                    throw new NotImplementedException("GOTOURL");
                })
            },
            {
                "GOTOURLFRAME", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.String);
                    var register2 = getStack(iptTracking, IptVariableTypes.String);

                    //$window.open(register2.Value, register1.Value);

                    throw new NotImplementedException("GOTOURLFRAME");
                })
            },
            {
                "KILLUSER", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.SEND, new MSG_Header
                    //{
                    //    eventType = Core.Enums.EventTypes.MSG_KILLUSER,
                    //    protocolSend = new MSG_KILLUSER
                    //    {
                    //        targetID = (uint)register.Value,
                    //    },
                    //});

                    throw new NotImplementedException("KILLUSER");
                })
            },
            {
                "CLEARLOG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    //$scope.model.Interface.LogList = [];

                    throw new NotImplementedException("CLEARLOG");
                })
            },

            #endregion End Misc Commands

            #endregion

            #region Iptscrae Version 2

            #region Start Misc Commands

            {
                "ADDHEADER",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ADDHEADER"); })
            },
            {
                "ALERTBOX",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ALERTBOX"); })
            },
            {
                "BITAND",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("BITAND"); })
            },
            {
                "BITOR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("BITOR"); })
            },
            {
                "BITSHIFTLEFT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("BITSHIFTLEFT"); })
            },
            {
                "BITSHIFTRIGHT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("BITSHIFTRIGHT"); })
            },
            {
                "BITXOR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("BITXOR"); })
            },
            {
                "CHARTONUM",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("CHARTONUM"); })
            },
            {
                "CLEARTOOLTIP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("CLEARTOOLTIP"); })
            },
            {
                "CLIENTID",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("CLIENTID"); })
            },
            {
                "CONFIRMBOX",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("CONFIRMBOX"); })
            },
            {
                "DECODEURL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("DECODEURL"); })
            },
            {
                "ENCODEURL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ENCODEURL"); })
            },
            {
                "GETTIMEZONE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETTIMEZONE"); })
            },
            {
                "HASHTOJSON",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("HASHTOJSON"); })
            },
            {
                "HTTPGET",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("HTTPGET"); })
            },
            {
                "HTTPPOST",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("HTTPPOST"); })
            },
            {
                "HTTPCANCEL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("HTTPCANCEL"); })
            },
            {
                "JSONTOHASH",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("JSONTOHASH"); })
            },
            {
                "LOADWEBSITE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("LOADWEBSITE"); })
            },
            {
                "ISFUNCTION",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ISFUNCTION"); })
            },
            {
                "ISKEYDOWN",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ISKEYDOWN"); })
            },
            {
                "ISSOUNDPLAYING",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ISSOUNDPLAYING"); })
            },
            {
                "OPENPALACE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("OPENPALACE"); })
            },
            {
                "PALACECHAT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PALACECHAT"); })
            },
            {
                "PROMPT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PROMPT"); })
            },
            {
                "NBRSERVERUSERS",
                (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        //(int)sessionState.ServerPopulation,
                        0));
                })
            },
            {
                "NEWHASH",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("NEWHASH"); })
            },
            {
                "NUMTOCHAR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("NUMTOCHAR"); })
            },
            {
                "REGEXP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REGEXP"); })
            },
            {
                "REGEXPREPLACE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REGEXPREPLACE"); })
            },
            {
                "REMOVEHEADER",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REMOVEHEADER"); })
            },
            {
                "REPLACE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REPLACE"); })
            },
            {
                "REPLACEALL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REPLACEALL"); })
            },
            {
                "RESETHEADERS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("RESETHEADERS"); })
            },
            {
                "SELECTFILE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SELECTFILE"); })
            },
            {
                "SETCURSOR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETCURSOR"); })
            },
            {
                "SETCURSORPIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETCURSORPIC"); })
            },
            {
                "SETTOOLTIP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETTOOLTIP"); })
            },
            {
                "SETUSERNAME",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETUSERNAME"); })
            },
            {
                "SOUNDOPEN",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDOPEN"); })
            },
            {
                "SOUNDPLAY",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDPLAY"); })
            },
            //{ "SOUNDPAUSE", (IptCommandFnc)((iptTracking, recursionDepth) =>
            //    {
            //        throw new NotImplementedException("CLEARLOG");
            //    }) },
            {
                "SOUNDSEEK",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDSEEK"); })
            },
            {
                "SOUNDSTOP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDSTOP"); })
            },
            {
                "SOUNDISPLAYING",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDISPLAYING"); })
            },
            {
                "SOUNDGETPOSITION",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDGETPOSITION"); })
            },
            {
                "SOUNDLENGTH",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDLENGTH"); })
            },
            {
                "SOUNDPLAYFROM",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SOUNDPLAYFROM"); })
            },
            {
                "STOPALARM",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("STOPALARM"); })
            },
            {
                "STOPALARMS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("STOPALARMS"); })
            },
            {
                "TEXTSPEECH",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("TEXTSPEECH"); })
            },
            {
                "TIMEREXEC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("TIMEREXEC"); })
            },
            {
                "UPDATELATER",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("UPDATELATER"); })
            },
            {
                "UPDATENOW",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("UPDATENOW"); })
            },
            {
                "WHOCOLOR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("WHOCOLOR"); })
            },
            {
                "WHOFACE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("WHOFACE"); })
            },

            #endregion End Misc Commands

            #region Start Spot Commands

            {
                "ADDPIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ADDPIC"); })
            },
            {
                "ADDPICNAME",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ADDPICNAME"); })
            },
            {
                "ADDSPOT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ADDSPOT"); })
            },
            {
                "AUTOUSERLAYER",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("AUTOUSERLAYER"); })
            },
            {
                "CACHESCRIPT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("CACHESCRIPT"); })
            },
            {
                "FILEDATE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("FILEDATE"); })
            },
            {
                "FILEDELETE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("FILEDELETE"); })
            },
            {
                "FILEEXISTS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("FILEEXISTS"); })
            },
            {
                "GETBUBBLESTYLE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETBUBBLESTYLE"); })
            },
            {
                "GETPICANGLE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICANGLE"); })
            },
            {
                "GETPICDIMENSIONS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICDIMENSIONS"); })
            },
            {
                "GETPICLOC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICLOC"); })
            },
            {
                "GETSPOTLOC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETSPOTLOC"); })
            },
            {
                "GETSPOTTEXTSIZE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETSPOTTEXTSIZE"); })
            },
            {
                "GETSPOTPOINTS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETSPOTPOINTS"); })
            },
            {
                "GETSPOTOPTIONS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETSPOTOPTIONS"); })
            },
            {
                "GETROOMOPTIONS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETROOMOPTIONS"); })
            },
            {
                "GETPICBRIGHTNESS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICBRIGHTNESS"); })
            },
            {
                "GETPICOPACITY",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICOPACITY"); })
            },
            {
                "GETPICSATURATION",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICSATURATION"); })
            },
            {
                "GETPICNAME",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICNAME"); })
            },
            {
                "GETPICPIXEL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("GETPICPIXEL"); })
            },
            {
                "HIDEAVATARS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("HIDEAVATARS"); })
            },
            {
                "IMAGETOMEDIA",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("IMAGETOMEDIA"); })
            },
            {
                "ISRIGHTCLICK",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ISRIGHTCLICK"); })
            },
            {
                "INSERTPIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("INSERTPIC"); })
            },
            {
                "LOCINSPOT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("LOCINSPOT"); })
            },
            {
                "MEDIAADDRESS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("MEDIAADDRESS"); })
            },
            {
                "NBRPICFRAMES",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("NBRPICFRAMES"); })
            },
            {
                "PAUSEPIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PAUSEPIC"); })
            },
            {
                "REMOVEPIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REMOVEPIC"); })
            },
            {
                "REMOVESPOT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REMOVESPOT"); })
            },
            {
                "RESUMEPIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("RESUMEPIC"); })
            },
            {
                "ROOMPICNAME",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ROOMPICNAME"); })
            },
            {
                "ROOMWIDTH",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ROOMWIDTH"); })
            },
            {
                "ROOMHEIGHT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("ROOMHEIGHT"); })
            },
            {
                "SETPICANGLE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICANGLE"); })
            },
            {
                "SETPICFRAME",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICFRAME"); })
            },
            {
                "SETLOCLOCAL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETLOCLOCAL"); })
            },
            {
                "SETPICLOCLOCAL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICLOCLOCAL"); })
            },
            {
                "SETPICBRIGHTNESS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICBRIGHTNESS"); })
            },
            {
                "SETPICBLUR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICBLUR"); })
            },
            {
                "SETPICCONTRAST",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICCONTRAST"); })
            },
            {
                "SETPICHUE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICHUE"); })
            },
            {
                "SETPICOPACITY",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICOPACITY"); })
            },
            {
                "SETPICSATURATION",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETPICSATURATION"); })
            },
            {
                "SETSPOTCLIP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTCLIP"); })
            },
            {
                "SETSPOTCURVE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTCURVE"); })
            },
            {
                "SETSPOTFONT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTFONT"); })
            },
            {
                "SETSPOTNAMELOCAL",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTNAMELOCAL"); })
            },
            {
                "SETSPOTOPTIONS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTOPTIONS"); })
            },
            {
                "SETSPOTPICMODE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTPICMODE"); })
            },
            {
                "SETSPOTPOINTS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTPOINTS"); })
            },
            {
                "SETSPOTSCRIPT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTSCRIPT"); })
            },
            {
                "SETSPOTSTYLE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SETSPOTSTYLE"); })
            },
            {
                "SHOWAVATARS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SHOWAVATARS"); })
            },
            {
                "WEBEMBED",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("WEBEMBED"); })
            },
            {
                "WEBLOCATION",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("WEBLOCATION"); })
            },
            {
                "WEBTITLE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("WEBTITLE"); })
            },
            {
                "WEBSCRIPT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("WEBSCRIPT"); })
            },

            #endregion End Spot Commands

            #region Start Prop Commands

            {
                "IMAGETOPROP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("IMAGETOPROP"); })
            },
            {
                "LOADPROPS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("LOADPROPS"); })
            },
            {
                "LOOSEPROP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("LOOSEPROP"); })
            },
            {
                "LOOSEPROPIDX",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("LOOSEPROPIDX"); })
            },
            {
                "LOOSEPROPPOS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("LOOSEPROPPOS"); })
            },
            {
                "MOVELOOSEPROP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("MOVELOOSEPROP"); })
            },
            {
                "NBRLOOSEPROPS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("NBRLOOSEPROPS"); })
            },
            {
                "PROPDIMENSIONS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PROPDIMENSIONS"); })
            },
            {
                "PROPOFFSETS",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PROPOFFSETS"); })
            },
            {
                "REMOVELOOSEPROP",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("REMOVELOOSEPROP"); })
            },

            #endregion End Prop Commands

            #region Start Paint Commands

            {
                "OVAL", (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("OVAL"); })
            },
            {
                "PENOPACITY",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENOPACITY"); })
            },
            {
                "PENFILLCOLOR",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENFILLCOLOR"); })
            },
            {
                "PENFILLOPACITY",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENFILLOPACITY"); })
            },
            {
                "POLYGON",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("POLYGON"); })
            },
            {
                "DRAWTEXT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("DRAWTEXT"); })
            },
            {
                "PENFONT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENFONT"); })
            },
            {
                "PENBOLD",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENBOLD"); })
            },
            {
                "PENUNDERLINE",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENUNDERLINE"); })
            },
            {
                "PENITALIC",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("PENITALIC"); })
            },
            {
                "SHOWPAINT",
                (IptCommandFnc)((iptTracking, recursionDepth) => { throw new NotImplementedException("SHOWPAINT"); })
            },

            #endregion End Paint Commands

            #endregion

            #region Iptscrae Version 3

            #region Start Message Commands

#if DEBUG
            {
                "DEBUGMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    LoggerHub.Current.Debug(register.Value.ToString());
                })
            },
            {
                "CONSOLEMSG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    LoggerHub.Current.Console(register.Value.ToString());
                })
            },
            {
                "CONNECT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.String);

                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //ThreadManager.Current.Enqueue(ThreadQueues.Network, null, sessionState, NetworkCommandTypes.CONNECT, register.Value.ToString());
                })
            },
            {
                "DISCONNECT", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    if (!iptTracking.Variables.TryGetValue("SESSIONSTATE", out var metaVariable) ||
                        metaVariable.Variable.GetValue<IUserSessionState<IApp>>() is not IUserSessionState<IApp> sessionState) return;

                    //NetworkManager.Current.Disconnect(sessionState);
                })
            },
#endif

            #endregion End Message Commands

            {
                "TRYCATCH", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Atomlist);
                    var register2 = getStack(iptTracking, IptVariableTypes.Atomlist);

                    try
                    {
                        Executor(register1.Value as IptAtomList, iptTracking, recursionDepth + 1);
                    }
                    catch (Exception ex)
                    {
                        LoggerHub.Current.Error(ex);

                        iptTracking.Variables["ERRORMSG"] = new IptMetaVariable
                        {
                            Variable = new IptVariable(
                                IptVariableTypes.String,
                                ex.Message),
                        };
                        iptTracking.Variables["ERRORMSG"].Flags |= IptMetaVariableFlags.IsReadOnly;

                        Executor(register2.Value as IptAtomList, iptTracking, recursionDepth + 1);
                    }
                })
            },
            {
                "READONLY", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = popStack(iptTracking, IptVariableTypes.Variable);

                    var key = register.Value?.ToString();

                    if (!iptTracking.Variables.ContainsKey(key) ||
                        iptTracking.Variables[key].IsSpecial) return;

                    iptTracking.Variables["ERRORMSG"].Flags |= IptMetaVariableFlags.IsReadOnly;
                })
            },

            #region Start Math Commands

            {
                "PI", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)(Math.PI * 1000000)));
                })
            },
            {
                "ABS", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        Math.Abs((int)register.Value)));
                })
            },
            {
                "AVG", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Array);

                    var values = (register.Value as IptAtomList)
                        .Select(v =>
                        {
                            if (v.Type != IptVariableTypes.Integer)
                                throw new Exception($"Wrong datatype {v.Type}...");

                            return (decimal)v.Value;
                        })
                        .ToList();
                    var avg = values.Sum() / values.Count;

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)avg));
                })
            },
            {
                "POW", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.Integer);
                    var register2 = getStack(iptTracking, IptVariableTypes.Integer);

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        Math.Pow((int)register2.Value, (int)register1.Value)));
                })
            },
            {
                "SUM", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Array);

                    var values = (register.Value as IptAtomList)
                        .Select(v =>
                        {
                            if (v.Type != IptVariableTypes.Integer)
                                throw new Exception($"Wrong datatype {v.Type}...");

                            return (decimal)v.Value;
                        })
                        .ToList();

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)values.Sum()));
                })
            },
            {
                "MIN", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Array);

                    var values = (register.Value as IptAtomList)
                        .Select(v =>
                        {
                            if (v.Type != IptVariableTypes.Integer)
                                throw new Exception($"Wrong datatype {v.Type}...");

                            return (decimal)v.Value;
                        })
                        .ToList();

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)values.Min()));
                })
            },
            {
                "MAX", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register = getStack(iptTracking, IptVariableTypes.Array);

                    var values = (register.Value as IptAtomList)
                        .Select(v =>
                        {
                            if (v.Type != IptVariableTypes.Integer)
                                throw new Exception($"Wrong datatype {v.Type}...");

                            return (decimal)v.Value;
                        })
                        .ToList();

                    iptTracking.Stack.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        (int)values.Max()));
                })
            },
            {
                "COALESCE", (IptCommandFnc)((iptTracking, recursionDepth) =>
                {
                    var register1 = getStack(iptTracking, IptVariableTypes.String, IptVariableTypes.Bool,
                        IptVariableTypes.Integer, IptVariableTypes.Decimal);
                    var register2 = getStack(iptTracking, IptVariableTypes.Array);

                    var atom = register2.Value as IptAtomList;

                    for (var j = 0; j < atom.Count; j++)
                        if (atom[j].Type != register1.Type ||
                            atom[j].Value != register1.Value)
                        {
                            iptTracking.Stack.Push(atom[j]);

                            break;
                        }
                })
            },

            #endregion End Math Commands

            #endregion

            #endregion
        });

    protected static readonly ConcurrentDictionary<string, IptOperator> IptOperators =
        new(new Dictionary<string, IptOperator>
        {
            #region Iptscrae Operators

            {
                "~", new IptOperator
                {
                    Flags = IptOperatorFlags.Unary | IptOperatorFlags.Push | IptOperatorFlags.NOT,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            ~(int)register1.Value)
                }
            },
            {
                "!", new IptOperator
                {
                    Flags = IptOperatorFlags.Unary | IptOperatorFlags.Boolean | IptOperatorFlags.Push |
                            IptOperatorFlags.NOT,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value == 0 ? 1 : 0)
                }
            },
            {
                "++", new IptOperator
                {
                    Flags = IptOperatorFlags.Unary | IptOperatorFlags.Assigning | IptOperatorFlags.Math |
                            IptOperatorFlags.Add,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value + 1)
                }
            },
            {
                "--", new IptOperator
                {
                    Flags = IptOperatorFlags.Unary | IptOperatorFlags.Assigning | IptOperatorFlags.Math |
                            IptOperatorFlags.Subtract,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value - 1)
                }
            },
            {
                "-", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Math | IptOperatorFlags.Subtract,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value - (int)register2.Value)
                }
            },
            {
                "*", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Math | IptOperatorFlags.Multiply,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value * (int)register2.Value)
                }
            },
            {
                "/", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Math | IptOperatorFlags.Divide,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value / (int)register2.Value)
                }
            },
            {
                "%", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Math | IptOperatorFlags.Modulo,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value % (int)register2.Value)
                }
            },
            {
                "&", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Concate,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.String,
                            string.Concat(register1.Value, register2.Value))
                }
            },
            {
                "&&", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Boolean | IptOperatorFlags.AND,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value != 0 && (int)register2.Value != 0 ? 1 : 0)
                }
            },
            {
                "||", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Boolean | IptOperatorFlags.OR,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value != 0 || (int)register2.Value != 0 ? 1 : 0)
                }
            },
            {
                "=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning,
                    OpFnc = (register1, register2) => register2
                }
            },
            {
                "~=", new IptOperator
                {
                    Flags = IptOperatorFlags.Unary | IptOperatorFlags.Assigning | IptOperatorFlags.NOT,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            ~(int)register1.Value)
                }
            },
            {
                "|=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning | IptOperatorFlags.Boolean | IptOperatorFlags.OR,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value != 0 || (int)register2.Value != 0 ? 1 : 0)
                }
            },
            {
                "-=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning | IptOperatorFlags.Math | IptOperatorFlags.Subtract,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value - (int)register2.Value)
                }
            },
            {
                "+=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning | IptOperatorFlags.Math | IptOperatorFlags.Add,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value + (int)register2.Value)
                }
            },
            {
                "*=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning | IptOperatorFlags.Math | IptOperatorFlags.Multiply,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value * (int)register2.Value)
                }
            },
            {
                "/=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning | IptOperatorFlags.Math | IptOperatorFlags.Divide,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value / (int)register2.Value)
                }
            },
            {
                "%=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning | IptOperatorFlags.Math | IptOperatorFlags.Modulo,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Integer,
                            (int)register1.Value % (int)register2.Value)
                }
            },
            {
                ">", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.GreaterThan,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value > (int)register2.Value ? 1 : 0)
                }
            },
            {
                ">=", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.GreaterThan |
                            IptOperatorFlags.EqualTo,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value >= (int)register2.Value ? 1 : 0)
                }
            },
            {
                "<", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.LessThan,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value < (int)register2.Value ? 1 : 0)
                }
            },
            {
                "<=", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.LessThan |
                            IptOperatorFlags.EqualTo,
                    OpFnc = (register1, register2) =>
                        new IptVariable(
                            IptVariableTypes.Bool,
                            (int)register1.Value <= (int)register2.Value ? 1 : 0)
                }
            },
            {
                "!=", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.NotEqualTo,
                    OpFnc = (register1, register2) =>
                    {
                        return register1.Type switch
                        {
                            IptVariableTypes.String => new IptVariable(
                                IptVariableTypes.Bool,
                                register1.Value.ToString() != register2.Value.ToString() ? 1 : 0),
                            IptVariableTypes.Bool or
                                IptVariableTypes.Integer => new IptVariable(
                                    IptVariableTypes.Bool,
                                    (int)register1.Value != (int)register2.Value ? 1 : 0),
                            _ => throw new Exception($"Wrong datatype {register1.Type}...")
                        };
                    }
                }
            },
            {
                "<>", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.NotEqualTo,
                    OpFnc = (register1, register2) =>
                    {
                        return register1.Type switch
                        {
                            IptVariableTypes.String => new IptVariable(
                                IptVariableTypes.Bool,
                                register1.Value.ToString() != register2.Value.ToString() ? 1 : 0),
                            IptVariableTypes.Bool or
                                IptVariableTypes.Integer => new IptVariable(
                                    IptVariableTypes.Bool,
                                    (int)register1.Value != (int)register2.Value ? 1 : 0),
                            _ => throw new Exception($"Wrong datatype {register1.Type}...")
                        };
                    }
                }
            },
            {
                "==", new IptOperator
                {
                    Flags = IptOperatorFlags.Push | IptOperatorFlags.Comparator | IptOperatorFlags.EqualTo,
                    OpFnc = (register1, register2) =>
                    {
                        return register1.Type switch
                        {
                            IptVariableTypes.String => new IptVariable(
                                IptVariableTypes.Bool,
                                register1.Value.ToString() == register2.Value.ToString() ? 1 : 0),
                            IptVariableTypes.Bool or
                                IptVariableTypes.Integer => new IptVariable(
                                    IptVariableTypes.Bool,
                                    (int)register1.Value == (int)register2.Value ? 1 : 0),
                            _ => throw new Exception($"Wrong datatype {register1.Type}...")
                        };
                    }
                }
            },
            {
                "+", new IptOperator
                {
                    Flags = IptOperatorFlags.Push |
                            IptOperatorFlags.Math |
                            IptOperatorFlags.Add |
                            IptOperatorFlags.Concate,
                    OpFnc = (register1, register2) =>
                    {
                        return register1.Type switch
                        {
                            IptVariableTypes.String => new IptVariable(
                                IptVariableTypes.String,
                                $"{register1.Value}{register2.Value}"),
                            IptVariableTypes.Bool or
                                IptVariableTypes.Integer => new IptVariable(
                                    IptVariableTypes.Integer,
                                    (int)register1.Value + (int)register2.Value),
                            _ => throw new Exception($"Wrong datatype {register1.Type}...")
                        };
                    }
                }
            },
            {
                "&=", new IptOperator
                {
                    Flags = IptOperatorFlags.Assigning |
                            IptOperatorFlags.Boolean |
                            IptOperatorFlags.AND |
                            IptOperatorFlags.Concate,
                    OpFnc = (register1, register2) =>
                    {
                        return register1.Type switch
                        {
                            IptVariableTypes.String => new IptVariable(
                                IptVariableTypes.String,
                                $"{register1.Value}{register2.Value}"),
                            IptVariableTypes.Bool or
                                IptVariableTypes.Integer => new IptVariable(
                                    IptVariableTypes.Bool,
                                    (int)register1.Value != 0 && (int)register2.Value != 0 ? 1 : 0),
                            _ => throw new Exception($"Wrong datatype {register1.Type}...")
                        };
                    }
                }
            },

            #endregion
        });

    protected static IptVariable getStack(IptTracking iptTracking, params IptVariableTypes[] constraintTypes)
    {
        if (iptTracking.Stack.Count < 1) throw new Exception("Stack is empty...");

        var register = popStack(iptTracking);
        register = getVariable(iptTracking, register);

        if (constraintTypes.Length > 0 &&
            !constraintTypes.Contains(register.Type)) throw new Exception($"Wrong datatype {register.Type}...");

        return register;
    }

    protected static IptVariable popStack(IptTracking iptTracking, params IptVariableTypes[] constraintTypes)
    {
        if (iptTracking.Stack.Count < 1) throw new Exception("Stack is empty...");

        var register = iptTracking.Stack.Pop();

        if (constraintTypes.Length > 0 &&
            !constraintTypes.Contains(register.Type)) throw new Exception($"Wrong datatype {register.Type}...");

        return register;
    }

    protected static IptVariable getVariable(IptTracking iptTracking, IptVariable variable)
    {
        if (variable.Type != IptVariableTypes.Variable) return variable;

        var key = variable.Value?.ToString()?.Trim();
        if (string.IsNullOrWhiteSpace(key)) throw new NullReferenceException();

        if (iptTracking.Variables.TryGetValue(key, out var value) &&
            value.Variable.Type != IptVariableTypes.Hidden)
            return value.Variable;

        return new IptVariable(
            IptVariableTypes.Integer,
            0);
    }

    protected static void setVariable(IptTracking iptTracking, IptVariable destination, IptVariable variable,
        int recursionDepth = 0, IptMetaVariableFlags flags = IptMetaVariableFlags.None)
    {
        var key = destination.Value?.ToString();
        if (string.IsNullOrWhiteSpace(key)) throw new NullReferenceException();

        if (iptTracking.Variables.TryGetValue(key, out var value))
        {
            if (value.IsReadOnly)
                throw new Exception($"{key} IsReadOnly...");

            if (value.IsSpecial &&
                value.Variable.Type != variable.Type)
                throw new Exception($"Wrong datatype {value.Variable.Type}, {variable.Type}...");
            value.Variable = variable;
        }
        else
        {
            iptTracking.Variables[key] = new IptMetaVariable(recursionDepth, variable, flags);
        }
    }

    protected static void Operator(IptTracking iptTracking, string opKey, int recursionDepth = 0)
    {
        if (!IptOperators.TryGetValue(opKey, out var op)) return;

        var flags = op.Flags;

        var register1 = popStack(iptTracking);
        if ((flags & IptOperatorFlags.Assigning) == IptOperatorFlags.Assigning)
        {
            if (register1.Type != IptVariableTypes.Variable)
                throw new Exception($"Wrong datatype {register1.Value}...");
            if (iptTracking.Variables[register1.Value.ToString()].IsReadOnly)
                throw new Exception($"{register1.Value} IsReadOnly...");
        }

        var originalRegister1 = register1;
        var errorDataType1 = true;

        var register2 = (IptVariable?)null;
        var errorDataType2 = true;

        register1 = getVariable(iptTracking, register1);

        if ((flags & (IptOperatorFlags.Boolean | IptOperatorFlags.Math)) != 0 &&
            new[] { IptVariableTypes.Bool, IptVariableTypes.Integer }.Contains(register1.Type))
            errorDataType1 = false;
        else if ((flags & IptOperatorFlags.Concate) == IptOperatorFlags.Concate &&
                 register1.Type == IptVariableTypes.String)
            errorDataType1 = false;

        // Exclude Unary Operators
        if ((flags & IptOperatorFlags.Unary) == 0)
        {
            register2 = popStack(iptTracking);
            register2 = getVariable(iptTracking, register2);

            if ((flags & (IptOperatorFlags.Boolean | IptOperatorFlags.Math)) != 0 &&
                new[]
                    { IptVariableTypes.Bool, IptVariableTypes.Integer }.Contains(register2.Type))
                errorDataType2 = false;
            else if ((flags & IptOperatorFlags.Concate) == IptOperatorFlags.Concate &&
                     register2.Type == IptVariableTypes.String)
                errorDataType2 = false;
            else if ((flags & IptOperatorFlags.Comparator) == IptOperatorFlags.Comparator &&
                     register1.Type == register2.Type &&
                     new[]
                             { IptVariableTypes.Bool, IptVariableTypes.Integer, IptVariableTypes.String }
                         .Contains(register1.Type))
            {
                errorDataType1 = false;
                errorDataType2 = false;
            }
        }
        else
        {
            errorDataType2 = false;
        }

        if ((flags & (IptOperatorFlags.Boolean |
                      IptOperatorFlags.Math |
                      IptOperatorFlags.Concate |
                      IptOperatorFlags.Comparator)) == 0)
        {
            errorDataType1 = false;
            errorDataType2 = false;
        }

        if (errorDataType1 &&
            errorDataType2)
            throw new Exception($"Wrong datatypes {register1.Type} {opKey} {register2.Type}...");

        if (errorDataType1) throw new Exception($"Wrong datatype {register1.Type}...");

        if (errorDataType2) throw new Exception($"Wrong datatype {register2.Type}...");

        var result = (IptVariable?)null;

        if (op.OpFnc != null)
            result = op.OpFnc(register1, register2);

        if (result == null)
            throw new Exception($"Unexpected result {register1.Value} {opKey} {register2.Value}...");

        if ((flags & IptOperatorFlags.Push) == IptOperatorFlags.Push)
            iptTracking.Stack.Push(result);
        else if ((flags & IptOperatorFlags.Assigning) == IptOperatorFlags.Assigning)
            setVariable(iptTracking, originalRegister1, result, recursionDepth);
    }

    public static void Executor(IptAtomList AtomList, IptTracking iptTracking, int recursionDepth = 0)
    {
        for (var j = 0; j < AtomList.Count; j++)
        {
            var key = AtomList[j].Value?.ToString();

            if (key == null) continue;
            switch (AtomList[j].Type)
            {
                case IptVariableTypes.Command:
                    iptTracking.Flags.SetBit(IptTrackingFlags.Return, false);

                    ((IptCommandFnc)IptCommands[key])(iptTracking, recursionDepth);

                    if (IptTrackingFlags.Return.IsSet(iptTracking.Flags)) return;

                    break;
                case IptVariableTypes.Operator:
                    Operator(iptTracking, key, recursionDepth);

                    break;
                default:
                    iptTracking.Stack.Push(AtomList[j]);

                    break;
            }
        }

        // Garbage Collection:
        foreach (var key in iptTracking.Variables.Keys)
        {
            var value = iptTracking.Variables[key];
            var flags = value.Flags;

            if ((flags & IptMetaVariableFlags.IsGlobal) == 0 &&
                (flags & IptMetaVariableFlags.IsSpecial) == 0 &&
                (recursionDepth == 0 || value.Depth >= recursionDepth))
                iptTracking.Variables.Remove(key);
        }

        if (iptTracking.Stack.Count > 0)
        {
            if (iptTracking.Stack.Count > CONST_gStackMaxSize)
            {
                iptTracking.Stack.RemoveRange(CONST_gStackMaxSize, iptTracking.Stack.Count);

                LoggerHub.Current.Debug("Stack space exceeded...");
            }

            if (recursionDepth < 1)
            {
                LoggerHub.Current.Debug("Stack wasn't empty upon exit...");
            }
        }
    }

    public static IptAtomList Parse(IptTracking iptTracking, string str, bool hasEvents = true)
    {
        LoggerHub.Current.Debug($"PARSING: {str}");

        var chars = str?.ToCharArray() ?? [];
        var result = new IptAtomList();

        for (var j = 0; j < chars.Length;)
            if (char.IsWhiteSpace(chars[j]))
            {
                j++;
            }
            else if (chars[j] == ';')
            {
                for (j++; j < chars.Length; j++)
                    if (chars[j] == '\r' || chars[j] == '\n')
                        break;
            }
            else if (hasEvents && chars[j] == 'O' && chars[j + 1] == 'N' && char.IsWhiteSpace(chars[j + 2]))
            {
                var tokenStart = j += 3;
                var bracketDepth = 0;
                var arrayDepth = 0;

                for (; j < chars.Length; j++)
                    if (chars[j] == '{')
                        break;

                var eventName = str.Substr(tokenStart, j)
                    .Trim()
                    .ToUpperInvariant();

                if (!EventTypes.TryGetValue(eventName, out var eventType)) throw new Exception($"Unexpected event {eventName}...");

                tokenStart = j;

                for (; j < chars.Length; j++)
                    if (chars[j] == '"')
                    {
                        for (; j < chars.Length; j++)
                        {
                            if (chars[j] == '"') break;

                            if (chars[j] == '\\' && chars[j + 1] == '"') j++;
                        }
                    }
                    else if (chars[j] == '{')
                    {
                        bracketDepth++;

                        if (bracketDepth > CONST_gNestedAtomlistMaxDepth)
                            throw new Exception("Exceeded max atomlist depth...");
                    }
                    else if (chars[j] == '}')
                    {
                        if (bracketDepth > 0)
                        {
                            bracketDepth--;

                            if (bracketDepth == 0) break;
                        }
                        else
                        {
                            throw new Exception("Unexpected closing bracket \"}\"");
                        }
                    }
                    else if (chars[j] == '[')
                    {
                        arrayDepth++;

                        if (arrayDepth > CONST_gNestedArrayMaxDepth) throw new Exception("Exceeded max array depth...");
                    }
                    else if (chars[j] == ']')
                    {
                        if (arrayDepth > 0)
                            arrayDepth--;
                        else
                            throw new Exception("Unexpected closing bracket \"]\"");
                    }

                var atomlistStr = str.Substr(tokenStart + 1, j - 1);

                iptTracking.Events[eventType] = Parse(iptTracking, atomlistStr, false);

                j++;
            }
            else if (chars[j] == '"')
            {
                var tokenStart = j;

                for (j++; j < chars.Length; j++)
                {
                    if (chars[j] == '"') break;

                    if (chars[j] == '\\' && chars[j + 1] == '"') j++;
                }

                var substr = str.Substr(tokenStart + 1, j);

                result.Push(new IptVariable(
                    IptVariableTypes.String,
                    substr));

                j++;
            }
            else if (chars[j] == '{')
            {
                var bracketDepth = 0;
                var arrayDepth = 0;
                var tokenStart = j;

                for (; j < chars.Length; j++)
                    if (chars[j] == '"')
                    {
                        for (; j < chars.Length; j++)
                        {
                            if (chars[j] == '"') break;

                            if (chars[j] == '\\' && chars[j + 1] == '"') j++;
                        }
                    }
                    else if (chars[j] == '{')
                    {
                        bracketDepth++;

                        if (bracketDepth > CONST_gNestedAtomlistMaxDepth)
                            throw new Exception("Exceeded max atomlist depth...");
                    }
                    else if (chars[j] == '}')
                    {
                        if (bracketDepth > 0)
                        {
                            bracketDepth--;

                            if (bracketDepth == 0) break;
                        }
                        else
                        {
                            throw new Exception("Unexpected closing bracket \"}\"");
                        }
                    }
                    else if (chars[j] == '[')
                    {
                        arrayDepth++;

                        if (arrayDepth > CONST_gNestedArrayMaxDepth) throw new Exception("Exceeded max array depth...");
                    }
                    else if (chars[j] == ']')
                    {
                        if (arrayDepth > 0)
                            arrayDepth--;
                        else
                            throw new Exception("Unexpected closing bracket \"]\"");
                    }

                var atomlistContents = str.Substr(tokenStart + 1, j);

                result.Push(new IptVariable(
                    IptVariableTypes.Atomlist,
                    Parse(iptTracking, atomlistContents, false)));

                j++;
            }
            else if (chars[j] == '[')
            {
                var bracketDepth = 0;
                var arrayDepth = 0;
                var tokenStart = j;

                for (; j < chars.Length; j++)
                    if (chars[j] == '"')
                    {
                        for (; j < chars.Length; j++)
                        {
                            if (chars[j] == '"') break;

                            if (chars[j] == '\\' && chars[j + 1] == '"') j++;
                        }
                    }
                    else if (chars[j] == '{')
                    {
                        bracketDepth++;

                        if (bracketDepth > CONST_gNestedAtomlistMaxDepth)
                            throw new Exception("Exceeded max atomlist depth...");
                    }
                    else if (chars[j] == '}')
                    {
                        if (bracketDepth > 0)
                            bracketDepth--;
                        else
                            throw new Exception("Unexpected closing bracket \"}\"");
                    }
                    else if (chars[j] == '[')
                    {
                        arrayDepth++;

                        if (arrayDepth > CONST_gNestedArrayMaxDepth) throw new Exception("Exceeded max array depth...");
                    }
                    else if (chars[j] == ']')
                    {
                        if (arrayDepth > 0)
                        {
                            arrayDepth--;

                            if (arrayDepth == 0) break;
                        }
                        else
                        {
                            throw new Exception("Unexpected closing bracket \"]\"");
                        }
                    }

                var arrayContents = str.Substr(tokenStart + 1, j);

                result.Push(new IptVariable(
                    IptVariableTypes.Array,
                    Parse(iptTracking, arrayContents, false)));

                j++;
            }
            else if (char.IsDigit(chars[j]) ||
                     (chars[j] == '-' && char.IsDigit(chars[j + 1])))
            {
                var tokenStart = j;

                for (j++; j < chars.Length; j++)
                    if (!char.IsDigit(chars[j]) ||
                        char.IsWhiteSpace(chars[j]))
                        break;

                var token = str.Substr(tokenStart, j);

                try
                {
                    var value = int.Parse(token);

                    result.Push(new IptVariable(
                        IptVariableTypes.Integer,
                        value));
                }
                catch

                {
                    throw new Exception($"Unexpected {token}...");
                }
            }
            else if (new[]
                     {
                         '~', '!', '=', '+', '-', '*', '/', '%', '<', '>', '&', '|'
                     }.Contains(chars[j]))
            {
                if (j + 1 < chars.Length && chars[j + 1] == '=')
                {
                    var key = $"{chars[j]}{chars[j + 1]}";

                    if (IptOperators.ContainsKey(key))
                        result.Push(new IptVariable(
                            IptVariableTypes.Operator,
                            key));

                    j += 2;
                }
                else if (
                    j + 1 < chars.Length &&
                    ((chars[j] == '&' && chars[j + 1] == '&') ||
                     (chars[j] == '|' && chars[j + 1] == '|') ||
                     (chars[j] == '<' && chars[j + 1] == '>') ||
                     (chars[j] == '+' && chars[j + 1] == '+') ||
                     (chars[j] == '-' && chars[j + 1] == '-')))
                {
                    var key = $"{chars[j]}{chars[j + 1]}";

                    if (IptOperators.ContainsKey(key))
                        result.Push(new IptVariable(
                            IptVariableTypes.Operator,
                            key));

                    j += 2;
                }
                else
                {
                    var key = $"{chars[j]}";

                    if (IptOperators.ContainsKey(key))
                        result.Push(new IptVariable(
                            IptVariableTypes.Operator,
                            key));

                    j++;
                }
            }

            else
            {
                var tokenStart = j;

                for (j++; j < chars.Length; j++)
                    if (!char.IsLetterOrDigit(chars[j]) ||
                        char.IsWhiteSpace(chars[j]))
                        break;

                var token = str.Substr(tokenStart, j);
                var commmand = token.ToUpperInvariant();

                if (IptCommands.ContainsKey(commmand))
                {
                    while (IptCommands.ContainsKey(commmand) &&
                           IptCommands[commmand].GetType() == StringExts.Types.String)
                        commmand = IptCommands[commmand].ToString().ToUpperInvariant();

                    if (IptCommands.ContainsKey(commmand))
                    {
                        result.Push(new IptVariable(
                            IptVariableTypes.Command,
                            commmand));

                        j++;
                    }
                }
                else
                {
                    result.Push(new IptVariable(
                        IptVariableTypes.Variable,
                        token));

                    //j++;
                }
            }

        return result;
    }

    public static void RegisterAlias(params KeyValuePair<string, string>[] aliases)
    {
        foreach (var alias in aliases)
            IptCommands.TryAdd(alias.Key, alias.Value);
    }

    public static void RegisterCommand(params KeyValuePair<string, IptCommandFnc>[] commands)
    {
        foreach (var cmd in commands)
            IptCommands.TryAdd(cmd.Key, cmd.Value);
    }

    public static void RegisterOperator(params KeyValuePair<string, IptOperator>[] operators)
    {
        foreach (var op in operators)
            IptOperators.TryAdd(op.Key, op.Value);
    }

    public static void UnregisterCommand(params string[] commands)
    {
        foreach (var cmd in commands)
            IptCommands.TryRemove(cmd, out var _);
    }

    public static void UnregisterOperator(params string[] operators)
    {
        foreach (var op in operators)
            IptOperators.TryRemove(op, out var _);
    }
}
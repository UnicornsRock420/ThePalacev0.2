using ThePalace.Core.Attributes.Core;
using ThePalace.Core.Attributes.Serialization;

namespace ThePalace.Core.Enums.Palace;

[ByteSize(4)]
public enum EventTypes : uint
{
    #region Assets
    [Mnemonic("aAst")]
    MSG_ASSETNEW = 0x61417374,
    [Mnemonic("qAst")]
    MSG_ASSETQUERY = 0x71417374,
    [Mnemonic("rAst")]
    MSG_ASSETREGI = 0x72417374,
    [Mnemonic("sAst")]
    MSG_ASSETSEND = 0x73417374,
    [Mnemonic("dPrp")]
    MSG_PROPDEL = 0x64507270,
    [Mnemonic("mPrp")]
    MSG_PROPMOVE = 0x6D507270,
    [Mnemonic("nPrp")]
    MSG_PROPNEW = 0x6E507270,
    [Mnemonic("sPrp")]
    MSG_PROPSETDESC = 0x73507270,
    #endregion
    #region Auth
    [Mnemonic("susr")]
    MSG_SUPERUSER = 0x73757372,
    [Mnemonic("auth")]
    MSG_AUTHENTICATE = 0x61757468,
    [Mnemonic("autr")]
    MSG_AUTHRESPONSE = 0x61757472,
    #endregion
    #region Communications
    [Mnemonic("gmsg")]
    MSG_GMSG = 0x676D7367,
    [Mnemonic("rmsg")]
    MSG_RMSG = 0x726D7367,
    [Mnemonic("smsg")]
    MSG_SMSG = 0x736D7367,
    [Mnemonic("talk")]
    MSG_TALK = 0x74616C6B,
    [Mnemonic("whis")]
    MSG_WHISPER = 0x77686973,
    [Mnemonic("wmsg")]
    MSG_WMSG = 0x776D7367,
    [Mnemonic("xwis")]
    MSG_XWHISPER = 0x78776973,
    [Mnemonic("xtlk")]
    MSG_XTALK = 0x78746C6B,
    #endregion
    #region Media
    [Mnemonic("fnfe")]
    MSG_FILENOTFND = 0x666e6665,
    [Mnemonic("qFil")]
    MSG_FILEQUERY = 0x7146696C,
    [Mnemonic("sFil")]
    MSG_FILESEND = 0x7346696C,
    #endregion
    #region Network
    [Mnemonic("rep2")]
    MSG_ALTLOGONREPLY = 0x72657032,
    [Mnemonic("blow")]
    MSG_BLOWTHRU = 0x626C6F77,
    [Mnemonic("durl")]
    MSG_DISPLAYURL = 0x6475726C,
    [Mnemonic("ryit")]
    MSG_DIYIT = 0x72796974,
    [Mnemonic("HTTP")]
    MSG_HTTPSERVER = 0x48545450,
    [Mnemonic("cLog")]
    MSG_INITCONNECTION = 0x634C6F67,
    [Mnemonic("regi")]
    MSG_LOGON = 0x72656769,
    [Mnemonic("bye ")]
    MSG_LOGOFF = 0x62796520,
    [Mnemonic("ping")]
    MSG_PING = 0x70696E67,
    [Mnemonic("pong")]
    MSG_PONG = 0x706F6E67,
    [Mnemonic("resp")]
    MSG_RESPORT = 0x72657370,
    [Mnemonic("navR")]
    MSG_ROOMGOTO = 0x6E617652,
    [Mnemonic("tiyr")]
    MSG_TIYID = 0x74697972,
    #endregion
    #region Room
    [Mnemonic("lock")]
    MSG_DOORLOCK = 0x6C6F636B,
    [Mnemonic("unlo")]
    MSG_DOORUNLOCK = 0x756E6C6F,
    [Mnemonic("draw")]
    MSG_DRAW = 0x64726177,
    [Mnemonic("dPct")]
    MSG_PICTDEL = 0x46505371,
    [Mnemonic("pLoc")]
    MSG_PICTMOVE = 0x704C6F63,
    [Mnemonic("nPct")]
    MSG_PICTNEW = 0x6E506374,
    [Mnemonic("sPct")]
    MSG_PICTSETDESC = 0x73506374,
    [Mnemonic("nRom")]
    MSG_ROOMNEW = 0x6E526F6D,
    [Mnemonic("ofNr")]
    MSG_ROOMINFO = 0x6F664E72,
    [Mnemonic("sRom")]
    MSG_ROOMSETDESC = 0x73526F6D,
    [Mnemonic("room")]
    MSG_ROOMDESC = 0x726F6F6D,
    [Mnemonic("endr")]
    MSG_ROOMDESCEND = 0x656E6472,
    [Mnemonic("dSpo")]
    MSG_SPOTDEL = 0x6F705364,
    [Mnemonic("ofNs")]
    MSG_SPOTINFO = 0x6F664E73,
    [Mnemonic("sLoc")]
    MSG_SPOTMOVE = 0x636F4C73,
    [Mnemonic("nSpo")]
    MSG_SPOTNEW = 0x6F70536E,
    [Mnemonic("sSpo")]
    MSG_SPOTSETDESC = 0x6F705373,
    [Mnemonic("sSta")]
    MSG_SPOTSTATE = 0x73537461,
    #endregion
    #region Server
    [Mnemonic("sInf")]
    MSG_EXTENDEDINFO = 0x73496e66,
    [Mnemonic("kill")]
    MSG_KILLUSER = 0x6B696C6C,
    [Mnemonic("rLst")]
    MSG_LISTOFALLROOMS = 0x724C7374,
    [Mnemonic("uLst")]
    MSG_LISTOFALLUSERS = 0x754C7374,
    [Mnemonic("sErr")]
    MSG_NAVERROR = 0x73457272,
    [Mnemonic("NOOP")]
    MSG_NOOP = 0x4E4F4F50,
    [Mnemonic("down")]
    MSG_SERVERDOWN = 0x646F776E,
    [Mnemonic("sinf")]
    MSG_SERVERINFO = 0x73696E66,
    [Mnemonic("init")]
    MSG_SERVERUP = 0x696E6974,
    [Mnemonic("vers")]
    MSG_VERSION = 0x76657273,
    #endregion
    #region Users
    [Mnemonic("usrC")]
    MSG_USERCOLOR = 0x75737243,
    [Mnemonic("usrD")]
    MSG_USERDESC = 0x75737244,
    [Mnemonic("wprs")]
    MSG_USERENTER = 0x77707273,
    [Mnemonic("eprs")]
    MSG_USEREXIT = 0x65707273,
    [Mnemonic("usrF")]
    MSG_USERFACE = 0x75737246,
    [Mnemonic("rprs")]
    MSG_USERLIST = 0x72707273,
    [Mnemonic("log ")]
    MSG_USERLOG = 0x6C6F6720,
    [Mnemonic("uLoc")]
    MSG_USERMOVE = 0x754C6F63,
    [Mnemonic("nprs")]
    MSG_USERNEW = 0x6E707273,
    [Mnemonic("usrN")]
    MSG_USERNAME = 0x7573724E,
    [Mnemonic("usrP")]
    MSG_USERPROP = 0x75737250,
    [Mnemonic("uSta")]
    MSG_USERSTATUS = 0x75537461,
    #endregion
};
namespace ThePalace.Core.Entities.Scripting;

public delegate void IptCommandFnc(IptTracking iptTracking, int recursionDepth);
public delegate IptVariable IptOperatorFnc(IptVariable register1, IptVariable register2);
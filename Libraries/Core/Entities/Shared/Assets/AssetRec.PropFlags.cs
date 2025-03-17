using Lib.Core.Enums;

namespace Lib.Core.Entities.Shared.Assets;

public partial class AssetRec
{
    #region PropFlags & PropFormat

    public PropFormats PropFormat;

    public bool IsLegacy16Bit => (AssetDesc.PropFlags & 0xFFC1) == (int)PropFormats.PF_16Bit;

    //public bool IsLegacyS20Bit => (PropFormat & (int)PropFormats.PF_S20Bit) != 0;
    public bool IsLegacyS20Bit => PropFormats.PF_S20Bit.IsSet(PropFormat);

    //public bool IsLegacy20Bit => (PropFormat & (int)PropFormats.PF_20Bit) != 0;
    public bool IsLegacy20Bit => PropFormats.PF_20Bit.IsSet(PropFormat);

    //public bool IsLegacy32Bit => (PropFormat & (int)PropFormats.PF_32Bit) != 0;
    public bool IsLegacy32Bit => PropFormats.PF_32Bit.IsSet(PropFormat);

    //public bool IsCustom32Bit => (AssetDesc.PropFlags & (int)PropFormats.PF_Custom32Bit) != 0;
    public bool IsCustom32Bit => PropFormats.PF_Custom32Bit.IsSet((PropFormats)AssetDesc.PropFlags);
    public bool LoResIsHead => LoResPropFlags.PF_Head.IsSet((LoResPropFlags)AssetDesc.PropFlags);
    public bool LoResIsGhost => LoResPropFlags.PF_Ghost.IsSet((LoResPropFlags)AssetDesc.PropFlags);
    public bool LoResIsRare => LoResPropFlags.PF_Rare.IsSet((LoResPropFlags)AssetDesc.PropFlags);
    public bool LoResIsAnimate => LoResPropFlags.PF_Animate.IsSet((LoResPropFlags)AssetDesc.PropFlags);

    public bool LoResIsPalindrome => LoResPropFlags.PF_Palindrome.IsSet((LoResPropFlags)AssetDesc.PropFlags);

    public bool LoResIsBounce => LoResPropFlags.PF_Bounce.IsSet((LoResPropFlags)AssetDesc.PropFlags);
    public bool HiResIsHead => HiResPropFlags.PF_Head.IsSet((HiResPropFlags)AssetDesc.PropFlags);
    public bool HiResIsGhost => HiResPropFlags.PF_Ghost.IsSet((HiResPropFlags)AssetDesc.PropFlags);
    public bool HiResIsRare => false;
    public bool HiResIsAnimate => HiResPropFlags.PF_Animate.IsSet((HiResPropFlags)AssetDesc.PropFlags);
    public bool HiResIsPalindrome => false;
    public bool HiResIsBounce => HiResPropFlags.PF_Bounce.IsSet((HiResPropFlags)AssetDesc.PropFlags);

    public bool IsHead
    {
        get
        {
            if (IsLegacy16Bit ||
                IsLegacy20Bit ||
                IsLegacyS20Bit ||
                IsLegacy32Bit)
                return LoResIsHead;
            return HiResIsHead;
        }
    }

    public bool IsGhost
    {
        get
        {
            if (IsLegacy16Bit ||
                IsLegacy20Bit ||
                IsLegacyS20Bit ||
                IsLegacy32Bit)
                return LoResIsGhost;
            return HiResIsGhost;
        }
    }

    public bool IsRare
    {
        get
        {
            if (IsLegacy16Bit ||
                IsLegacy20Bit ||
                IsLegacyS20Bit ||
                IsLegacy32Bit)
                return LoResIsRare;
            return HiResIsRare;
        }
    }

    public bool IsAnimate
    {
        get
        {
            if (IsLegacy16Bit ||
                IsLegacy20Bit ||
                IsLegacyS20Bit ||
                IsLegacy32Bit)
                return LoResIsAnimate;
            return HiResIsAnimate;
        }
    }

    public bool IsPalindrome
    {
        get
        {
            if (IsLegacy16Bit ||
                IsLegacy20Bit ||
                IsLegacyS20Bit ||
                IsLegacy32Bit)
                return LoResIsPalindrome;
            return HiResIsPalindrome;
        }
    }

    public bool IsBounce
    {
        get
        {
            if (IsLegacy16Bit ||
                IsLegacy20Bit ||
                IsLegacyS20Bit ||
                IsLegacy32Bit)
                return LoResIsBounce;
            return HiResIsBounce;
        }
    }

    #endregion
}
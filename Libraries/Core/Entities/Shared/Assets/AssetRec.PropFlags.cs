using ThePalace.Core.Enums.Palace;
using sint32 = System.Int32;
using uint16 = System.UInt16;

namespace ThePalace.Core.Entities.Shared.Assets
{
    public partial class AssetRec
    {
        #region PropFlags & PropFormat
        public sint32 PropFormat;
        public bool IsLegacy16Bit => (AssetDesc.PropFlags & 0xFFC1) == (int)PropFormats.PF_16Bit;
        //public bool IsLegacyS20Bit => (PropFormat & (int)PropFormats.PF_S20Bit) != 0;
        public bool IsLegacyS20Bit => PropFormats.PF_S20Bit.IsSet<PropFormats, sint32, sint32>(PropFormat);
        //public bool IsLegacy20Bit => (PropFormat & (int)PropFormats.PF_20Bit) != 0;
        public bool IsLegacy20Bit => PropFormats.PF_20Bit.IsSet<PropFormats, sint32, sint32>(PropFormat);
        //public bool IsLegacy32Bit => (PropFormat & (int)PropFormats.PF_32Bit) != 0;
        public bool IsLegacy32Bit => PropFormats.PF_32Bit.IsSet<PropFormats, sint32, sint32>(PropFormat);
        //public bool IsCustom32Bit => (AssetDesc.PropFlags & (int)PropFormats.PF_Custom32Bit) != 0;
        public bool IsCustom32Bit => PropFormats.PF_Custom32Bit.IsSet<PropFormats, uint16, uint16>(AssetDesc.PropFlags);
        public bool LoResIsHead => LoResPropFlags.PF_Head.IsSet<LoResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool LoResIsGhost => LoResPropFlags.PF_Ghost.IsSet<LoResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool LoResIsRare => LoResPropFlags.PF_Rare.IsSet<LoResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool LoResIsAnimate => LoResPropFlags.PF_Animate.IsSet<LoResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool LoResIsPalindrome => LoResPropFlags.PF_Palindrome.IsSet<LoResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool LoResIsBounce => LoResPropFlags.PF_Bounce.IsSet<LoResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool HiResIsHead => HiResPropFlags.PF_Head.IsSet<HiResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool HiResIsGhost => HiResPropFlags.PF_Ghost.IsSet<HiResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool HiResIsRare => false;
        public bool HiResIsAnimate => HiResPropFlags.PF_Animate.IsSet<HiResPropFlags, uint16, uint16>(AssetDesc.PropFlags);
        public bool HiResIsPalindrome => false;
        public bool HiResIsBounce => HiResPropFlags.PF_Bounce.IsSet<HiResPropFlags, uint16, uint16>(AssetDesc.PropFlags);

        public bool IsHead
        {
            get
            {
                if (IsLegacy16Bit ||
                    IsLegacy20Bit ||
                    IsLegacyS20Bit ||
                    IsLegacy32Bit)
                    return LoResIsHead;
                else
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
                else
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
                else
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
                else
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
                else
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
                else
                    return HiResIsBounce;
            }
        }
        #endregion
    }
}
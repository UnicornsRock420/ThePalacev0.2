using System.Runtime.Serialization;
using ThePalace.Core.Interfaces;
using ThePalace.Network.Enums;
using sint32 = System.Int32;
using uint16 = System.UInt16;

namespace ThePalace.Core.Entities.Shared
{
    public partial class AssetRec
    {
        #region PropFlags & PropFormat
        [IgnoreDataMember]
        public sint32 PropFormat;
        [IgnoreDataMember]
        public bool IsLegacy16Bit => (Desc.PropFlags & 0xFFC1) == (int)PropFormats.PF_16Bit;
        [IgnoreDataMember]
        public bool IsLegacyS20Bit => (PropFormat & (int)PropFormats.PF_S20Bit) != 0;
        [IgnoreDataMember]
        public bool IsLegacy20Bit => (PropFormat & (int)PropFormats.PF_20Bit) != 0;
        [IgnoreDataMember]
        public bool IsLegacy32Bit => (PropFormat & (int)PropFormats.PF_32Bit) != 0;
        [IgnoreDataMember]
        public bool IsCustom32Bit => (Desc.PropFlags & (int)PropFormats.PF_Custom32Bit) != 0;
        [IgnoreDataMember]
        public bool LoResIsHead => LoResPropFlags.PF_Head.IsBit<LoResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool LoResIsGhost => LoResPropFlags.PF_Ghost.IsBit<LoResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool LoResIsRare => LoResPropFlags.PF_Rare.IsBit<LoResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool LoResIsAnimate => LoResPropFlags.PF_Animate.IsBit<LoResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool LoResIsPalindrome => LoResPropFlags.PF_Palindrome.IsBit<LoResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool LoResIsBounce => LoResPropFlags.PF_Bounce.IsBit<LoResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool HiResIsHead => HiResPropFlags.PF_Head.IsBit<HiResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool HiResIsGhost => HiResPropFlags.PF_Ghost.IsBit<HiResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool HiResIsRare => false;
        [IgnoreDataMember]
        public bool HiResIsAnimate => HiResPropFlags.PF_Animate.IsBit<HiResPropFlags, uint16, uint16>(Desc.PropFlags);
        [IgnoreDataMember]
        public bool HiResIsPalindrome => false;
        [IgnoreDataMember]
        public bool HiResIsBounce => HiResPropFlags.PF_Bounce.IsBit<HiResPropFlags, uint16, uint16>(Desc.PropFlags);

        [IgnoreDataMember]
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
        [IgnoreDataMember]
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
        [IgnoreDataMember]
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
        [IgnoreDataMember]
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
        [IgnoreDataMember]
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
        [IgnoreDataMember]
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
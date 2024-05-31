import {
  AppConfigState,
  DeviceType,
  LayoutMode,
  PageAnim,
  SideTheme,
  ThemeMode,
} from '@/store/types'

export const projectName = 'ZCYMES Admin'

export default {
  theme: ThemeMode.LIGHT,
  sideTheme: SideTheme.WHITE,
  themeColor: 'rgb(24, 160, 88)',
  layoutMode: LayoutMode.LTR,
  sideWidth: 210,
  deviceType: DeviceType.PC,
  pageAnim: PageAnim.OPACITY,
  isFixedNavBar: true,
  isCollapse: false,
  actionBar: {
    isShowSearch: false,
    isShowMessage: false,
    isShowRefresh: true,
    isShowFullScreen: true,
  },
} as AppConfigState

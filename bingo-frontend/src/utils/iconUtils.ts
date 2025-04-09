import {
  faHome,
  faBookmark,
  faChartLine,
  faChartPie,
  faChartBar,
  faUsers,
  faComments,
  faCalendar,
  faCog,
  faFolder
} from '@fortawesome/free-solid-svg-icons'

import type { IconDefinition } from '@fortawesome/free-solid-svg-icons'

const iconMap: Record<string, IconDefinition> = {
  'faHome': faHome,
  'faBookmark': faBookmark,
  'faChartLine': faChartLine,
  'faChartPie': faChartPie,
  'faChartBar': faChartBar,
  'faUsers': faUsers,
  'faComments': faComments,
  'faCalendar': faCalendar,
  'faCog': faCog,
  'faFolder': faFolder,
};

export const getIcon = (iconName: string): IconDefinition => {
  return iconMap[iconName] || faHome;
};

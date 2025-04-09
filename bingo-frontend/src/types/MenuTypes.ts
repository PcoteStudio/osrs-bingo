export interface MenuItemType {
  id: string;
  label: string;
  icon: string;
  action: string;
  isSubmenu?: boolean;
  expanded?: boolean;
  items?: MenuItemType[];
}

export interface MenuSectionType {
  id: string;
  title: string;
  expanded: boolean;
  items: MenuItemType[];
}

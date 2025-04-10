export interface MenuItemType {
  id: string;
  label: string;
  action: string;
  icon?: string;
  isSubmenu?: boolean;
  expanded?: boolean;
  active?: boolean;
  items?: MenuItemType[];
}

export interface MenuSectionType {
  id: string;
  title: string;
  expanded?: boolean;
  bottom?: boolean;
  hideSection?: boolean;
  items: MenuItemType[];
}

export interface ICategory {
  id: number;
  name: string;
  image: string;
  description: string;
  parentCategoryId: number | null;
}

export interface ICategoryUpdate {
  name: string;
  description: string;
  image: File | null;
  parentCategoryId: number | null;
}

export interface ICategoryCreate {
  name: string;
  description: string;
  image: File | null;
  parentCategoryId: number | null;
}

export interface ICategorySelection {
  categories: ICategory[];
  parentCategories: ICategory[];
}

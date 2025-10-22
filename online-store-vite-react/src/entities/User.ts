import { IComment } from "./Comment";

export interface IUser {
  id: string;
  email: string;
  image: string;
  userName: string;
  lastName: string;
  firstName: string;
  dateCreated: Date;
  phoneNumber: string;
  sex: string;
  position: string;
  roles: string[];
  comments: IComment[];
  addressId: number;
  tablePermissions: string[];
}

export interface IUserUpdate {
  userName: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  position: string;
  image: File | null;
  roles: string[];
}

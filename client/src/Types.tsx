import React from "react"

export interface IUserInfoContext {
  UserInfo: UserInfo,
  SetUserInfo: React.Dispatch<React.SetStateAction<UserInfo>>
}


export interface UserInfo {
  email: string,
  role: string,
  storeName: string
}

export interface ProductInfo {
  id: string,
  name: string,
  description: string,
  imageUrl: string,
  price: number,
  quantity: number,
  category: string,
  storeId: string,
}
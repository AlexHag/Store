import Header from "../Components/Header";
import { useState, useEffect, useContext } from 'react';
// import { UserInfoContext } from '../App';
import { useNavigate } from "react-router-dom";
import { UserInfo } from "../Types";
import { useCookies } from "react-cookie";

function ProfilePage() {
  const userInfo: UserInfo = JSON.parse(localStorage.getItem('userInfo') || '{}') as UserInfo;
  const [cookies, setCookie, removeCookie] = useCookies(['Authorization']);
  const navigate = useNavigate();

  useEffect(() => {
    if(!userInfo.email) navigate("/");
  }, []);

  const handleLogout = () => {
    removeCookie("Authorization");
    localStorage.removeItem("userInfo");
    navigate("/");
    navigate(0);
  };

  return (
    <>
      <Header />
      <button onClick={handleLogout}>Log Out</button>
      <h1>Your Profile</h1>
    </>
  )
}

export default ProfilePage;
import Header from "../Components/Header";
import { useState, useEffect, useContext } from 'react';
import { UserInfoContext } from '../App';
import { useNavigate } from "react-router-dom";

function ProfilePage() {
  const userInfo = useContext(UserInfoContext);
  const navigate = useNavigate();

  useEffect(() => {
    if(!userInfo.email) navigate("/");
  }, []);

  const handleLogout = () => {
    localStorage.removeItem("Authorization");
    navigate(0);
    navigate("/");
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
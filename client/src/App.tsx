import React from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import { useState, useEffect, createContext, useContext } from 'react';
import { useCookies } from 'react-cookie';


import Header from './Components/Header';
import HomePage from './Pages/HomePage';
import LoginPage from './Pages/LoginPage';
import RegisterPage from './Pages/RegisterPage';
import ErrorOccuredPage from './Pages/ErrorOccuredPage';
import MyStorePage from './Pages/MyStorePage';
import ProfilePage from './Pages/ProfilePage';
import { UserInfo, IUserInfoContext } from './Types';

// export const UserInfoContext = createContext<IUserInfoContext>({UserInfo: {email: "", role: "", storeName: ""}, SetUserInfo: () => {}});

function App() {
  // const [userInfo, setUserInfo] = useState<UserInfo>({email: "", role: "", storeName: ""});
  // const [cookies, setCookie, removeCookie] = useCookies(['Authorization']);

  // useEffect(() => {
  //   console.log("getting user info from app.tsx")
  //   GetUserInfo();
  // }, []);

  // const GetUserInfo = async () => {
  //   if(!cookies['Authorization']) return;
  //   const response = await fetch('http://localhost:5046/api/userinfo', {
  //     headers: {
  //       'Authorization': 'Bearer ' + cookies['Authorization']
  //     }
  //   });
  //   if(response.status !== 200) return;
  //   setUserInfo(await response.json());
  // }

  return (
    // <UserInfoContext.Provider value={{UserInfo: userInfo, SetUserInfo: setUserInfo}} >
      <BrowserRouter> 
        <div className="App">
          <Routes> 
            <Route path="/" element={<HomePage />}></Route>
            <Route path="/Login" element={<LoginPage />}></Route>
            <Route path="/Register" element={<RegisterPage />}></Route>
            <Route path="/MyStore" element={<MyStorePage />}></Route>
            <Route path="/Profile" element={<ProfilePage />}></Route>
            <Route path="ErrorOccured" element={<ErrorOccuredPage />}></Route>
            <Route path='*' element={<ErrorOccuredPage />} />
          </Routes>  
        </div>
      </BrowserRouter>
    // </UserInfoContext.Provider>
  );
}

export default App;

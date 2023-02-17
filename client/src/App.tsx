import React from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import { useState, useEffect, createContext, useContext } from 'react';

import Header from './Components/Header';
import HomePage from './Pages/HomePage';
import LoginPage from './Pages/LoginPage';
import RegisterPage from './Pages/RegisterPage';
import ErrorOccuredPage from './Pages/ErrorOccuredPage';
import MyStorePage from './Pages/MyStorePage';
import ProfilePage from './Pages/ProfilePage';

interface UserInfo {
  email: string,
  role: string,
  storeName: string
}

export const UserInfoContext = createContext<UserInfo>({email: "", role: "", storeName: ""});

function App() {
  const [userInfoValue, setUserInfoValue] = useState<UserInfo>({email: "", role: "", storeName: ""});

  useEffect(() => {
    console.log("App.tsx use effect excecuting")
    GetUserInfo();
  }, []);

  const GetUserInfo = async () => {
    const authToken = localStorage.getItem("Authorization");
    if(!authToken) return;
    const response = await fetch('http://localhost:5046/api/userinfo', {
      headers: {
        'Authorization': 'Bearer ' + authToken
      }
    });
    if(response.status !== 200) return;
    setUserInfoValue(await response.json());
  }

  return (
    <UserInfoContext.Provider value={userInfoValue}>
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
    </UserInfoContext.Provider>
  );
}

export default App;

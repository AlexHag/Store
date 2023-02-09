import React from 'react';
import logo from './logo.svg';
import './App.css';
import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import HomePage from './Pages/HomePage';
import LoginPage from './Pages/LoginPage';
import RegisterPage from './Pages/RegisterPage';
import ErrorOccuredPage from './Pages/ErrorOccuredPage';

function App() {
  return (
    <BrowserRouter> 
      <div className="App">
        <div className="App-header">
            <ul><Link to="/">Home</Link></ul>
            <ul><Link to="/Login">Login</Link></ul>
            <ul><Link to="/Register">Register</Link></ul>
        </div>
        <Routes> 
          <Route path="/" element={<HomePage />}></Route>
          <Route path="/Login" element={<LoginPage />}></Route>
          <Route path="/Register" element={<RegisterPage />}></Route>
          <Route path="ErrorOccured" element={<ErrorOccuredPage />}></Route>
        </Routes>  
      </div>
      </BrowserRouter>
  );
}

export default App;

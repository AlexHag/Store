import { BrowserRouter, Link, Routes, Route } from 'react-router-dom';
import { UserInfoContext } from '../App';
import { useState, useContext } from 'react';

function Header() {
  const userInfo = useContext(UserInfoContext);
  if(userInfo.role === 'storeowner') {
    return (
    <div className="App-header">
      <ul><Link to="/">Home</Link></ul>
      <ul><Link to="/MyStore">My Store</Link></ul>
    </div>
    )
  }

  if(userInfo.role === 'user') {
    return (
    <div className="App-header">
      <ul><Link to="/">Home</Link></ul>
      <ul><Link to="/Profile">Profile</Link></ul>
    </div>
    )
  }

  return (
    <div className="App-header">
      <ul><Link to="/">Home</Link></ul>
      <ul><Link to="/Login">Login</Link></ul>
    </div>
  )
}

export default Header;
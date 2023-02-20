import { useState, useContext } from 'react';
import { useCookies } from 'react-cookie';
// import { UserInfoContext } from '../App';
import Header from '../Components/Header';
import { UserInfo } from '../Types';

function HomePage() {
  const userInfo: UserInfo = JSON.parse(localStorage.getItem('userInfo') || '{}') as UserInfo;

  const [helloMessage, setHelloMessage] = useState('');
  const [cookies, setCookie, removeCookie] = useCookies(['Authorization']);

  const handleHello = async () => {
    try {
      const response = await fetch('http://localhost:5046/api/secure', {
        headers: {
          'Authorization': 'Bearer ' + cookies['Authorization']
        }
      });
      if(response.ok) {
        setHelloMessage(await response.text());
      } else {
        setHelloMessage(`Request error, status: ${response.status}`);
      }
    } catch (error: any) {
      setHelloMessage(`Catch error : ${error}`);
    }
  }

  return (
    <>
      <Header />
      <h1>Home page</h1>
      <button onClick={handleHello}>Say Hello</button>
      <p>Response: {helloMessage}</p>
      <p>Context: {userInfo.email}</p>
      <p>Context: {userInfo.storeName}</p>
      <p>Local storage email: {userInfo.email}</p>
    </>
  )
}

export default HomePage;
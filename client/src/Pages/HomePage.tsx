import { useState, useContext } from 'react';
import { UserInfoContext } from '../App';
import Header from '../Components/Header';

function HomePage() {
  const userInfo = useContext(UserInfoContext);
  const [helloMessage, setHelloMessage] = useState('');
  
  const handleHello = async () => {
    try {
      const response = await fetch('http://localhost:5046/api/secure', {
        headers: {
          'Authorization': 'Bearer ' + localStorage.getItem("Authorization") || ''
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
    </>
  )
}

export default HomePage;
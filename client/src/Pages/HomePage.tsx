import { useState } from 'react';

function HomePage() {
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
      <h1>Home page</h1>
      <button onClick={handleHello}>Say Hello</button>
      <p>Response: {helloMessage}</p>
    </>
  )
}

export default HomePage;
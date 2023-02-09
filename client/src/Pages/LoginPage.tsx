import { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';

function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loginError, setLoginError] = useState('');
  let navigate = useNavigate();
  
  const handleLogin = async () => {
    setLoginError("Loading...");

    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json' },
      body: JSON.stringify({email: email, password: password})
    };

    const response = await fetch(`http://localhost:5046/api/login`, requestOptions);
    if(response.status === 200) {
      const jwtToken = await response.json();
      console.log("JWT:TOKEN from login: ", jwtToken['token']);
      localStorage.setItem('Authorization', jwtToken['token']);
      navigate("/");
    } else {
      setLoginError("Wrong email or password");
    }
  }

  return (
    <div className="login-page">
      <h1>Login page</h1>
      <input placeholder="Email" onChange={(e) => setEmail(e.target.value)}></input>
      <input placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
      <button onClick={handleLogin}>Login</button>
      <p style={{color: 'red'}}>{loginError}</p>
    </div>
    )
}
  
  export default LoginPage;
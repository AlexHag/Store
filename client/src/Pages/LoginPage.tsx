import { useState, useEffect } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import Header from '../Components/Header';

function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [loginError, setLoginError] = useState('');
  let navigate = useNavigate();
  
  const handleLogin = async (event: any) => {
    event.preventDefault();
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
      localStorage.setItem('Authorization', jwtToken['token']);
      navigate("/");
      navigate(0);
    } else {
      setLoginError("Wrong email or password");
    }
  }

  return (
    <div className="login-page">
      {/* <Header /> */}
      
      <h1>Login page</h1>
      <form onSubmit={handleLogin} className="auth-form">
        <input type="text" placeholder="Email" onChange={(e) => setEmail(e.target.value)}></input>
        <input type="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
        <button type="submit">Login</button>
      </form>
      <p style={{color: 'red'}}>{loginError}</p>
      <Link to="/Register">No account? Register</Link>
      <br></br>
      <Link to="/">Home</Link>
    </div>
    )
}
  
  export default LoginPage;
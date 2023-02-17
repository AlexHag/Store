import { useState, useEffect } from "react";
import { useCookies } from "react-cookie";
import { Link, useNavigate } from "react-router-dom";
import Header from "../Components/Header";

function RegisterPage() {
  let navigate = useNavigate();
  const [cookies, setCookie, removeCookie] = useCookies(['Authorization']);
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [role, setRole] = useState("user");
  const [storeName, setStoreName] = useState("");
  const [registerStatus, setRegisterStatus] = useState("");

  const handleRegister = async (event: any) => {
    event.preventDefault();
    if(email === "" || password === "" || confirmPassword === "" || role === "") {
      setRegisterStatus('Please submit all fields');
      return;
    }
    if(role === "storeowner" && storeName === "") {
      setRegisterStatus('Please submit all fields');
      return;
    }
    if(password !== confirmPassword) {
      setRegisterStatus('Password and email must match');
      return;
    }

    setRegisterStatus('Loading...');
    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json' },
      body: JSON.stringify({email: email, password: password, role: role, storename: storeName})
    };

    const response = await fetch(`http://localhost:5046/api/register`, requestOptions);
    if(response.status === 200) {
      const jwtToken = await response.json();
      localStorage.setItem('Authorization', jwtToken['token']);
      setCookie('Authorization', jwtToken['token']);
      navigate("/");
    } else {
      setRegisterStatus('Email already exists');
    }
  };

  useEffect(() => {
    setTimeout(() => {
      setRegisterStatus("");
    }, 5000)
  }, [registerStatus]);

  return (
    <div className="register-page">
      <h1>Register</h1>
      <form onSubmit={handleRegister} className="auth-form">
        <input type="text" placeholder="Email" onChange={(e) => setEmail(e.target.value)}></input>
        <input type="password" placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
        <input type="password" placeholder="Confirm Password" onChange={(e) => setConfirmPassword(e.target.value)}></input>
        
        <p>Choose a role:</p>
        <select defaultValue="user" name="role" id="cars" onChange={(e) => setRole(e.target.value)}>
          <option value="user">User</option>
          <option value="storeowner">Store Owner</option>
        </select>
        {role==='storeowner' && 
          <>
            <p>Choose a store name: </p>
            <input type="text" placeholder="Store Name" onChange={(e) => setStoreName(e.target.value)}></input>
          </>
        }

        <button type="submit">Register</button>
      </form>
      <p style={{color: 'red'}}>{registerStatus}</p>
      <Link to="/Login">Already have an account? Login</Link>
      <br></br>
      <Link to="/">Home</Link>
    </div>
  )
}
  
export default RegisterPage;
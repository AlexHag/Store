import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

function RegisterPage() {
  let navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [role, setRole] = useState("user");
  const [storeName, setStoreName] = useState("");
  const [registerStatus, setRegisterStatus] = useState("");

  const handleRegister = async () => {    
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
      console.log("JWT:TOKEN from login: ", jwtToken['token']);
      localStorage.setItem('Authorization', jwtToken['token']);
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

      <input placeholder="Email" onChange={(e) => setEmail(e.target.value)}></input>
      <input placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
      <input placeholder="Confirm Password" onChange={(e) => setConfirmPassword(e.target.value)}></input>
      
      <p>Choose a role:</p>
      <select defaultValue="user" name="role" id="cars" onChange={(e) => setRole(e.target.value)}>
        <option value="user">User</option>
        <option value="storeowner">Store Owner</option>
      </select>
      {role==='storeowner' && 
        <>
          <p>Choose a store name: </p>
          <input placeholder="Store Name" onChange={(e) => setStoreName(e.target.value)}></input>
        </>
      }

      <button onClick={handleRegister}>Register</button>
      <p style={{color: 'red'}}>{registerStatus}</p>
    </div>
  )
}
  
export default RegisterPage;
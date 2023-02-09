import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";

function RegisterPage() {
  let navigate = useNavigate();
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [role, setRole] = useState("user");
  const [registerError, setRegisterError] = useState("");

  const handleRegister = async () => {    
    if(email === "" || password === "" || confirmPassword === "" || role === "") {
      setRegisterError('Please submit all fields');
      return;
    }
    if(password !== confirmPassword) {
      setRegisterError('Password and email must match');
      return;
    }
    setRegisterError('Loading...');
    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json' },
      body: JSON.stringify({email: email, password: password, role: role})
    };

    const response = await fetch(`http://localhost:5046/api/register`, requestOptions);
    if(response.status === 200) {
      navigate('/');
      return
    } else {
      setRegisterError('Email already exists');
    }
  };

  useEffect(() => {
    setTimeout(() => {
      setRegisterError("");
    }, 5000)
  }, [registerError]);

  return (
    <div className="register-page">
      <h1>Register</h1>

      <input placeholder="Email" onChange={(e) => setEmail(e.target.value)}></input>
      <input placeholder="Password" onChange={(e) => setPassword(e.target.value)}></input>
      <input placeholder="Confirm Password" onChange={(e) => setConfirmPassword(e.target.value)}></input>
      
      <label htmlFor="role">Choose a role:</label>
      <select defaultValue="user" name="role" id="cars" onChange={(e) => setRole(e.target.value)}>
        <option value="user">User</option>
        <option value="storeowner">Store Owner</option>
      </select>

      <button onClick={handleRegister}>Register</button>
      <p style={{color: 'red'}}>{registerError}</p>
    </div>
  )
}
  
export default RegisterPage;
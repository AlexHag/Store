import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { UserInfoContext } from "../App";
import Header from "../Components/Header";

function MyStorePage() {
  const [showAddProduct, setShowAddProduct] = useState(false);
  const [newProductName, setNewProductName] = useState("");
  const [newProductDescription, setNewProductDescription] = useState("");
  const [newProductImgUrl, setNewProductImgUrl] = useState("");
  const [newProductPrice, setNewProductPrice] = useState("");
  const [newProductQuantity, setNewProductQuantity] = useState("");
  const [newProductCategory, setNewProductCategory] = useState("");



  const userInfo = useContext(UserInfoContext);
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("Authorization");
    navigate(0);
    navigate("/");
  };

  const handleAddProductSubmit = async () => {
    // const response = await fetch('http://localhost:5046/api/secure', {
    //   headers: {
    //     'Authorization': 'Bearer ' + localStorage.getItem("Authorization") || ''
    //   }
    // });
  }

  return (
    <>
      <Header />
      <h1>Your Store</h1>
      <button style={{width: "20%", padding: "10px"}} onClick={handleLogout}>Log Out</button>
        <br></br>
      <button style={{width: "20%", padding: "10px", margin: "1%"}} onClick={() => setShowAddProduct(!showAddProduct)}>
        {showAddProduct ? "Hide" : "Add products"}
      </button>
      {showAddProduct && 
      <>
        <form className="add-product-form" onSubmit={handleAddProductSubmit}>
          <input placeholder="Name" onChange={(e) => setNewProductName(e.target.value)}></input>
          <input placeholder="Description" onChange={(e) => setNewProductDescription(e.target.value)}></input>
          <input placeholder="Image url" onChange={(e) => setNewProductImgUrl(e.target.value)}></input>
          <input type="number" placeholder="Price" onChange={(e) => setNewProductPrice(e.target.value)}></input>
          <input type="number" placeholder="Quantity" onChange={(e) => setNewProductQuantity(e.target.value)}></input>
          <input type="number" placeholder="Category" onChange={(e) => setNewProductCategory(e.target.value)}></input>
          <button style={{padding: "10px 0px"}} type="button">Submit</button>
        </form>
      </>}
    </>
  );
}

export default MyStorePage;

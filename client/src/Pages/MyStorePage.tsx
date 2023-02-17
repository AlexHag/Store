import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { UserInfoContext } from "../App";
import Header from "../Components/Header";
import { ProductInfo } from "../Types";


function MyStorePage() {
  const userInfo = useContext(UserInfoContext);
  const navigate = useNavigate();
  const [MyProducts, setMyProducts] = useState<ProductInfo[]>([]);
  const [showAddProduct, setShowAddProduct] = useState(false);
  const [newProductName, setNewProductName] = useState("");
  const [newProductDescription, setNewProductDescription] = useState("");
  const [newProductImgUrl, setNewProductImgUrl] = useState("");
  const [newProductPrice, setNewProductPrice] = useState("");
  const [newProductQuantity, setNewProductQuantity] = useState("");
  const [newProductCategory, setNewProductCategory] = useState("");

  useEffect(() => {
    GetMyProducts();
  }, []);

  const GetMyProducts = async () => {
    const authToken = localStorage.getItem("Authorization");
    if(!authToken) return;
    const response = await fetch(`http://localhost:5046/api/Store/${userInfo.storeName}`, {
      headers: {
        'Authorization': 'Bearer ' + authToken
      }
    });
    if(response.status !== 200) return;
    setMyProducts(await response.json());
  };



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

  const dostuff = () => {
    console.log(MyProducts);
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
      <h2>Your Products</h2>
        {MyProducts.map(p => 
        <>
          <p>Name: {p.name}</p>
          <p>Description: {p.description}</p>
        </>
        )}
      <br></br>
      <button onClick={dostuff}>Do stuff</button>
    </>
  );
}

export default MyStorePage;

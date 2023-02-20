import { useEffect, useState } from "react";
import { useCookies } from "react-cookie";
import { useNavigate } from "react-router-dom";
import Header from "../Components/Header";
import { ProductInfo, UserInfo } from "../Types";


function MyStorePage() {
  const userInfo: UserInfo = JSON.parse(localStorage.getItem('userInfo') || '{}') as UserInfo;
  const [cookies, setCookie, removeCookie] = useCookies(['Authorization']);
  const navigate = useNavigate();

  useEffect(() => {
    if(!userInfo.email) navigate("/");
    GetMyProducts();
  }, []);

  const handleLogout = () => {
    removeCookie("Authorization");
    localStorage.removeItem("userInfo");
    navigate("/");
    navigate(0);
  };

  const [MyProducts, setMyProducts] = useState<ProductInfo[]>([]);
  const [showAddProduct, setShowAddProduct] = useState(false);
  const [addProductError, setAddProductError] = useState("");
  const [newProductName, setNewProductName] = useState("");
  const [newProductDescription, setNewProductDescription] = useState("");
  const [newProductImgUrl, setNewProductImgUrl] = useState("");
  const [newProductPrice, setNewProductPrice] = useState("");
  const [newProductQuantity, setNewProductQuantity] = useState("");
  const [newProductCategory, setNewProductCategory] = useState("");

  const GetMyProducts = async () => {
    
    const response = await fetch(`http://localhost:5046/api/Store/${userInfo.storeName}`, {
      headers: {
        'Authorization': 'Bearer ' + cookies['Authorization']
      }
    });
    if(response.status !== 200) return;
    setMyProducts(await response.json());
  };

  const handleAddProductSubmit = async (event: any) => {
    event.preventDefault();
    setAddProductError('Loading...');

    const requestOptions = {
      method: 'POST',
      headers: { 'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + cookies['Authorization'] },
      body: JSON.stringify({name: newProductName, description: newProductDescription, imageUrl: newProductImgUrl, price: newProductPrice, quantity: newProductQuantity, category: newProductCategory})
    };

    const response = await fetch('http://localhost:5046/api/Products/Add', requestOptions)
    if(response.status !== 201) {
      setAddProductError(`Error adding product: ${await response.text()} Status: ${response.status}`);
      return;
    }
    setAddProductError("");
    GetMyProducts();
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
          <button style={{padding: "10px 0px"}} type="submit">Submit</button>
        </form>
        <p style={{color: 'red'}}>{addProductError}</p>
      </>}
      <h2>Your Products</h2>
        {MyProducts.map(p => 
        <>
          <p>Name: {p.name}</p>
          <p>Description: {p.description}</p>
        </>
        )}
      <br></br>
    </>
  );
}

export default MyStorePage;

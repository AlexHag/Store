import { useState, useEffect } from 'react';

function CreateStorePage() {
  const [storeName, setStoreName] = useState("");
  const [storeError, setStoreError] = useState("");

  const handleCreateStore = async () => {
    if(storeName === "") {
      setStoreError("Store name cannot be empty");
      return;
    }
  }

  useEffect(() => {
    setTimeout(() => {
      setStoreError("");
    }, 5000)
  }, [storeError]);

  return (
    <div className="create-store-page">
      <h1>Create Your Store</h1>
      <input placeholder="Store name" onChange={e => setStoreName(e.target.value)}></input>
      <button onClick={handleCreateStore}>Submit</button>
      <p style={{color: 'red'}}>{storeError}</p>
    </div>
  )
}

export default CreateStorePage;
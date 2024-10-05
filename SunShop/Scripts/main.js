// set quantity cart
let quantityCart = 0;
const quantityCartItem = document.querySelector(".quantity-cart");
async function setQuantityCart() {
    const res = await httpGet("/api/cart/quantity");
    if (res.code === 200) {
        quantityCartItem.textContent = res.data;
    }
}

setQuantityCart();

// toast
function toggleToast(toast, type, message, delay) {
    if (toast) {
        toast.querySelector(".toast-body").textContent = message;           
        if (type === "success") {
            toast.classList.remove("error");
            toast.classList.add("success");
        }
        else {
            toast.classList.add("error");
            toast.classList.remove("success");
        }
        toast.classList.add("show");

        setTimeout(() => {
            toast.classList.remove("show");
        }, 1200)
    }
}

const toast = document.querySelector(".toast");

//toggleToast(toast, "error", "Thanj cong", 1200)

// add to cart
const btnsAddToCartHome = document.querySelectorAll(".btn-add-to-cart");
btnsAddToCartHome?.forEach((btnAddToCart) => {
    const productId = btnAddToCart.dataset.id;
    const dataRequest = {
        productId,
        quantity: 1
    }

    btnAddToCart.addEventListener("click", async () => {
        handleAddToCart(dataRequest)
    })
})

async function handleAddToCart(dataRequest) {
    const res = await httpPost("/api/cart", dataRequest);

    if (res.code === 201) {
        setQuantityCart();
        toggleToast(toast, "success", "Sản phẩm đã được thêm vào giỏ hàng thành công.", 1200);
    }

}

// add to cart detail page
const btnAddToCartDetailPage = document.querySelector(".btn-add-to-cart-detail");
const inputQuantityBuy = document.querySelector(".input-quantity-buy");
btnAddToCartDetailPage?.addEventListener("click", () => {
    const dataRequest = {
        productId: btnAddToCartDetailPage.dataset.id,
        quantity: inputQuantityBuy.value
    }

    handleAddToCart(dataRequest);
})

// increase by 1 number
const btnPlus = document.querySelector('.btn-plus')
const inputQuantityAddToCart = document.querySelector('.input-quantity-buy')
btnPlus?.addEventListener('click', () => {
    const inputNumber = Number(inputQuantityAddToCart.value) + 1;
    inputQuantityAddToCart.value = inputNumber  
})

//decrease by 1 number

const btnMinus = document.querySelector('.btn-minus')
btnMinus?.addEventListener('click', () => {
    if (inputQuantityAddToCart.value > 1) {
        const inputNumber = Number(inputQuantityAddToCart.value) - 1;
        inputQuantityAddToCart.value = inputNumber
    }
    
})
// function fetch data
async function httpPost(url, requestData) {
    const option = {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(requestData),
    }

    const res = await fetch(url, option).then(res => res.json());
    return res;
}

async function httpGet(url) {
    const option = {
        method: "GET",
    }

    const res = await fetch(url, option).then(res => res.json());
    return res;
}
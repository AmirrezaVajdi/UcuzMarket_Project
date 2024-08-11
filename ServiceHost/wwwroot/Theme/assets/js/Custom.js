const fullUrl = location.protocol + '//' + location.host;
const storageName = "cart-items";
const dayToCookeLife = 10;
async function SetDefaultAdderss(id) {
    var headersList = {
        "Content-Type": "application/json"
    }
    var data = { Id: id };
    var json = JSON.stringify(data);
    var response = await fetch(fullUrl + "/api/Delivery/SetDefaultAddress", {
        method: "POST",
        body: json,
        headers: headersList
    });
}

function AddToCart(Id, name, slug, picture, price, PriceWithDiscount) {

    let products = localStorage.getItem(storageName);

    if (products === null) {
        productsModel = [];
    }
    else {
        var productsModel = JSON.parse(products);
    }

    let count = document.getElementById("productCount").value;

    const currentProduct = productsModel.find(x => x.Id == Id);

    if (currentProduct !== undefined) {
        productsModel.find(x => x.Id === Id).count = parseInt(currentProduct.count) + parseInt(count);
    }
    else {
        if (PriceWithDiscount === undefined || PriceWithDiscount === "") {
            PriceWithDiscount = 0;
        }

        if (price === undefined || PriceWithDiscount === "") {
            price = 0;
        }

        const product = {
            Id,
            name,
            slug,
            picture,
            price,
            count,
            PriceWithDiscount
        }

        productsModel.push(product);
    }

    localStorage.setItem(storageName, JSON.stringify(productsModel));

    UpdateCart();
}

function UpdateCart() {
    let products = localStorage.getItem(storageName);

    products = JSON.parse(products);

    if (products === null) {
        products = [];
    }

    let prdouctCount = document.getElementById("cart_items_count").innerText = products.length;

    document.getElementById("cart_items_wrapper").innerHTML = "";

    let cart_items_wrapper = document.getElementById("cart_items_wrapper");

    products.forEach(function (product) {
        const res =
            `
             <div class="d-flex align-items-center">
                    <a class="position-relative flex-shrink-0" href="${fullUrl}/Product/${product.slug}">
                <span class="badge text-bg-danger position-absolute top-0 start-0 z-2 mt-0 ms-0">-${product.PriceWithDiscount}</span>
                        <img src="${fullUrl}/ProductPictures/${product.picture}" style=" width:100px;height:100px">
                    </a>
                    <div class="w-100 ps-3">
                        <h5 class="fs-sm fw-medium lh-base mb-2">
                            <a class="hover-effect-underline" href="${fullUrl}/Product/${product.slug}">${product.name}</a>
                        </h5>
                        <div class="h6 pb-1 mb-2">${product.price}</div>
                        <div class="float-end">
                            <button onclick="RemoveFromCart(${product.Id})" type="button" class="btn-close fs-sm" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-sm" data-bs-title="حذف" aria-label="حذف"></button>
                        </div>
                    </div>
             </div >
            `
        cart_items_wrapper.innerHTML += res;
    });
}

function RemoveFromCart(id) {

    let products = localStorage.getItem(storageName);

    if (products === null) {
        productsM = [];
    }
    else {
        var productsM = JSON.parse(products);
    }

    let index = productsM.findIndex(function (x) {
        return x.Id == id;
    });

    productsM.splice(index, 1);

    localStorage.setItem(storageName, JSON.stringify(productsM));

    UpdateCart();
}
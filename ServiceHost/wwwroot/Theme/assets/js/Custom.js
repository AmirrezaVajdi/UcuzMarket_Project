const fullUrl = location.protocol + '//' + location.host;
const cookieName = "cart-items";
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

    AddToCart();
}

function AddToCart(Id, name, slug, picture, price, PriceWithDiscount) {

    let products = getCookie(cookieName);

    if (products === null) {
        products = [];
    }
    else {
        products = JSON.parse(products);
    }

    let count = document.getElementById("productCount").value;

    const currentProduct = products.find(x => x.Id == Id);

    if (currentProduct !== undefined) {
        products.find(x => x.Id === Id).count = parseInt(currentProduct.count) + parseInt(count);
    }
    else {
        if (PriceWithDiscount === undefined) {
            PriceWithDiscount = 0;
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

        products.push(product);
    }

    setCookie(cookieName, JSON.stringify(products), dayToCookeLife);

    updateCart();
}

function updateCart() {
    let products = getCookie(cookieName);

    products = JSON.parse(products);

    let prdouctCount = document.getElementById("cart_items_count").innerText = products.length;

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
                        <div class="d-flex align-items-center justify-content-between">
                            <div class="count-input rounded-pill">
                                <button type="button" class="btn btn-icon btn-sm" data-decrement="" aria-label="Decrement quantity">
                                    <i class="ci-minus"></i>
                                </button>
                                <input type="number" class="form-control form-control-sm" value="3" readonly="">
                                <button type="button" class="btn btn-icon btn-sm" data-increment="" aria-label="Increment quantity">
                                    <i class="ci-plus"></i>
                                </button>
                            </div>
                            <button type="button" class="btn-close fs-sm" data-bs-toggle="tooltip" data-bs-custom-class="tooltip-sm" data-bs-title="Remove" aria-label="Remove from cart"></button>
                        </div>
                    </div>
             </div >
            `
        cart_items_wrapper.innerHTML += res;
    });

}


function setCookie(name, value, daysToLive) {
    // Encode value in order to escape semicolons, commas, and whitespace
    let cookie = name + "=" + encodeURIComponent(value);

    cookie += ";path=/;"

    if (typeof daysToLive === "number") {
        /* Sets the max-age attribute so that the cookie expires
        after the specified number of days */
        cookie += "; max-age=" + (daysToLive * 24 * 60 * 60);

        document.cookie = cookie;
    }
}

function getCookie(name) {
    // Split cookie string and get all individual name=value pairs in an array
    let cookieArr = document.cookie.split(";");

    // Loop through the array elements
    for (let i = 0; i < cookieArr.length; i++) {
        let cookiePair = cookieArr[i].split("=");

        /* Removing whitespace at the beginning of the cookie name
        and compare it with the given string */
        if (name == cookiePair[0].trim()) {
            // Decode the cookie value and return
            return decodeURIComponent(cookiePair[1]);
        }
    }

    // Return null if not found
    return null;
}
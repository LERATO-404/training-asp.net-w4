function createHeader(links, isLoggedIn) {
    const header = document.createElement('header');
    header.innerHTML = `
        <div id="menuLinks" class="container">
             <img src="/images/Logo.png" alt="PDT LOGO" class="logo">
            <nav>
                <ul>
                    ${links.map(link => `<li><a href="${link.url}">${link.text}</a></li>`).join('')}
                    ${isLoggedIn ? '<li class="nav nav-item"><a class="nav-link" id="logout4">Log out</a></li>' : '<li class="nav nav-item"><a class="nav-link" id="login4" href="AuthServices/UserManager/Login">Login / Register</a></li>'}
                </ul>
            </nav>
        </div>
    `;
    if (isLoggedIn) {
        const logoutButton = header.querySelector('#logout4');
        logoutButton.addEventListener('click', () => {
            window.location.href = "/AuthServices/UserManager/Logout";
        });
    }
    return header;
}

function createFooter() {
    const footer = document.createElement('footer');
    footer.className = 'footer';
    footer.innerHTML = `
        <div class="row">
            <div class="footer-col">
                <h4>Company</h4>
                <ul>
                    <li><a href="#">about us</a></li>
                    <li><a href="#">services</a></li>
                    <li><a href="#">FAQ</a></li>
                </ul>
            </div>

            <div class="footer-col">
                <h4>Contact Us</h4>
                <p>Email: contact@parceldeliverytracking.com</p>
                <p>Phone: +27 (123) 456-5677</p>
            </div>

            <div class="footer-col">
                <h4>Our Address</h4>
                <p>Parcel Tracking System</p>
                <p>1234 Delivery Avenue 567</p>
                <p>South City, 12345</p>
            </div>

            <div class="footer-col">
                <h4>follow us</h4>
                <div class="social-links">
                    <a href="#"><i class="fa-brands fa-facebook"></i></a>
                    <a href="#"><i class="fa-brands fa-x-twitter"></i></a>
                    <a href="#"><i class="fa-brands fa-square-instagram"></i></a>
                    <a href="#"><i class="fa-brands fa-linkedin"></i></a>
                </div>
            </div>
        </div>

        <p class="company-name">&copy; 2023 Parcel Delivery Tracking System</p>
    `;
    return footer;
}



function signInAndBegin() {
    const signInButton = document.getElementById("signInBeginBtn");

    signInButton.addEventListener("click", () => {
        window.location.href = "AuthServices/UserManager/Login";
    });
}

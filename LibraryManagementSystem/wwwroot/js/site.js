// SweetAlert for confirmations
document.addEventListener('DOMContentLoaded', function() {
    // Delete confirmation
    const deleteForms = document.querySelectorAll('form[action*="Delete"]');
    deleteForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            Swal.fire({
                title: 'Are you sure?',
                text: "You won't be able to revert this!",
                icon: 'warning',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, delete it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    this.submit();
                }
            });
        });
    });

    // Borrow confirmation
    const borrowForms = document.querySelectorAll('form[action*="Borrow"]');
    borrowForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            Swal.fire({
                title: 'Borrow Book?',
                text: "Are you sure you want to borrow this book?",
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, borrow it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    this.submit();
                }
            });
        });
    });

    // Return confirmation
    const returnForms = document.querySelectorAll('form[action*="Return"]');
    returnForms.forEach(form => {
        form.addEventListener('submit', function(e) {
            e.preventDefault();
            Swal.fire({
                title: 'Return Book?',
                text: "Are you sure you want to return this book?",
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, return it!'
            }).then((result) => {
                if (result.isConfirmed) {
                    this.submit();
                }
            });
        });
    });

    // Auto-hide alerts after 5 seconds
    const alerts = document.querySelectorAll('.alert');
    alerts.forEach(alert => {
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alert);
            bsAlert.close();
        }, 5000);
    });
});

// Enhanced JavaScript for new features

// Update wishlist count
async function updateWishlistCount() {
    try {
        const response = await fetch('/Wishlist/GetCount');
        if (response.ok) {
            const count = await response.json();
            const badge = document.querySelector('.wishlist-count');
            if (badge) {
                badge.textContent = count;
                badge.style.display = count > 0 ? 'inline' : 'none';
                
                // Add animation when count changes
                if (count > 0) {
                    badge.classList.add('animate__animated', 'animate__pulse');
                    setTimeout(() => {
                        badge.classList.remove('animate__animated', 'animate__pulse');
                    }, 1000);
                }
            }
        }
    } catch (error) {
        console.log('Could not fetch wishlist count');
    }
}

// Enhanced logout confirmation
document.addEventListener('DOMContentLoaded', function() {
    const logoutForm = document.getElementById('logoutForm');
    if (logoutForm) {
        logoutForm.addEventListener('submit', function(e) {
            e.preventDefault();
            Swal.fire({
                title: 'Ready to Leave?',
                text: "Are you sure you want to log out?",
                icon: 'question',
                showCancelButton: true,
                confirmButtonColor: '#3085d6',
                cancelButtonColor: '#d33',
                confirmButtonText: 'Yes, log out!',
                cancelButtonText: 'Cancel',
                background: '#1a1a1a',
                color: 'white',
                customClass: {
                    popup: 'animate__animated animate__zoomIn'
                }
            }).then((result) => {
                if (result.isConfirmed) {
                    // Show loading state
                    const button = this.querySelector('button[type="submit"]');
                    const originalText = button.innerHTML;
                    button.innerHTML = '<span class="loading-spinner"></span> Logging out...';
                    button.disabled = true;
                    
                    setTimeout(() => {
                        this.submit();
                    }, 1000);
                }
            });
        });
    }

    // Auto-update wishlist count every 30 seconds
    setInterval(updateWishlistCount, 30000);

    // Add animation to all book cards
    const bookCards = document.querySelectorAll('.book-card');
    bookCards.forEach((card, index) => {
        card.style.setProperty('--animation-order', index);
        card.classList.add('animate__animated', 'animate__fadeInUp');
    });

    // Enhanced search functionality
    const searchInput = document.querySelector('input[name="searchString"]');
    if (searchInput) {
        let searchTimeout;
        searchInput.addEventListener('input', function() {
            clearTimeout(searchTimeout);
            searchTimeout = setTimeout(() => {
                this.form.submit();
            }, 800);
        });
    }

    // Smooth scrolling for anchor links
    document.querySelectorAll('a[href^="#"]').forEach(anchor => {
        anchor.addEventListener('click', function (e) {
            const href = this.getAttribute('href').trim();
            if (href && href !== '#') {
                e.preventDefault();
                const target = document.querySelector(href);
                if (target) {
                    target.scrollIntoView({
                        behavior: 'smooth',
                        block: 'start'
                    });
                }
            }
        });
    });
});

// Notification functions
function showSuccess(message) {
    Swal.fire({
        title: 'Success!',
        text: message,
        icon: 'success',
        confirmButtonText: 'OK',
        timer: 3000,
        background: '#1a1a1a',
        color: 'white'
    });
}

function showError(message) {
    Swal.fire({
        title: 'Error!',
        text: message,
        icon: 'error',
        confirmButtonText: 'OK',
        background: '#1a1a1a',
        color: 'white'
    });
}

// Page loading animation
function showPageLoading() {
    const loadingDiv = document.createElement('div');
    loadingDiv.className = 'page-loading';
    loadingDiv.innerHTML = '<div class="loading-spinner-large"></div>';
    document.body.appendChild(loadingDiv);
}

function hidePageLoading() {
    const loadingDiv = document.querySelector('.page-loading');
    if (loadingDiv) {
        loadingDiv.remove();
    }
}

// Show loading when navigating between pages
document.addEventListener('DOMContentLoaded', function() {
    const links = document.querySelectorAll('a:not([target="_blank"])');
    links.forEach(link => {
        link.addEventListener('click', function(e) {
            if (this.href && !this.href.includes('javascript:')) {
                showPageLoading();
            }
        });
    });
});

// Hide loading when page is fully loaded
window.addEventListener('load', function() {
    hidePageLoading();
});
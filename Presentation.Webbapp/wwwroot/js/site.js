document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    // Open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]'); // Get all elements with data-modal="true"
    modalButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modalTarget = button.getAttribute('data-target'); // Get data-target
            const modal = document.querySelector(modalTarget); // Get modal element

            if (modal) {
                modal.style.display = 'flex'; // Show modal
            }
        });
    });

    // Close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]'); // Get all elements with data-close="true"
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal'); // Get closest parent with class modal

            if (modal) {
                modal.style.display = 'none'; // Hide modal

                // Clear formdata 
                modal.querySelectorAll('form').forEach(form => {
                    form.reset();

                    // Clear image preview
                    const imagePreview = form.querySelector('.image-preview');
                    if (imagePreview) {
                        imagePreview.src = '';

                        const imagePreviewer = form.querySelector('.image-previewer');
                        if (imagePreviewer) {
                            imagePreviewer.classList.remove('selected');
                        }
                    }
                });
            }
        });
    });

    // Handle image previewer
    document.querySelectorAll('.image-previewer').forEach(previewer => {
        const fileInput = previewer.querySelector('input[type="file"]');
        const imagePreview = previewer.querySelector('.image-preview');

        if (fileInput && imagePreview) {
            previewer.addEventListener('click', () => fileInput.click());

            fileInput.addEventListener('change', ({ target: { files } }) => {
                const file = files[0];
                if (file) processImage(file, imagePreview, previewer, previewSize);
            });
        }
    });

    // Handle submit forms
    //const forms = document.querySelectorAll('form');
    //forms.forEach(form => {
    //    form.addEventListener('submit', async (e) => {
    //        clearErrorMessages(form);

    //        const formData = new FormData(form);

    //        try {
    //            const res = await fetch(form.action, {
    //                method: 'post',
    //                body: formData
    //            });

    //            if (res.ok) {
    //                const modal = form.closest('.modal')
    //                if (modal)
    //                    modal.style.display = "none";

    //                window.location.reload()
    //            } else if (res.status === 400) {
    //                const data = await res.json();

    //                if (data.errors) {
    //                    Object.keys(data.errors).forEach(key => {
    //                        addErrorMessage(form, key, data.errors[key].join('\n'));
    //                    });
    //                }
    //            }
    //        } catch {
    //            console.log('Failed to submit form');
    //        }
    //    });
    //});

    function clearErrorMessages(form) {
        form.querySelectorAll('[data-val="true"]').forEach(input => {
            input.classList.remove('input-validation-error');
        });

        form.querySelectorAll('[data-valmsg-for]').forEach(span => {
            span.innerText = '';
            span.classList.remove('field-validation-error');
        });
    }

    function addErrorMessage(form, key, errorMessage) {
        const input = form.querySelector(`[name="${key}"]`);
        if (input) {
            input.classList.add('input-validation-error');
        }

        const span = form.querySelector(`[data-valmsg-for="${key}"]`);
        if (span) {
            span.innerText = errorMessage;
            span.classList.add('field-validation-error');
        }
    }

    async function loadImage(file) {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();

            reader.onerror = () => reject(new Error("Failed to load file"));
            reader.onload = (e) => {
                const img = new Image();
                img.onerror = () => reject(new Error("Failed to load image"));
                img.onload = () => resolve(img);
                img.src = e.target.result;
            };

            reader.readAsDataURL(file);
        });
    }

    async function processImage(file, imagePreview, previewer, previewSize = 150) {
        try {
            const img = await loadImage(file);
            const canvas = document.createElement('canvas');
            canvas.width = previewSize;
            canvas.height = previewSize;

            const ctx = canvas.getContext('2d');
            ctx.drawImage(img, 0, 0, previewSize, previewSize);

            imagePreview.src = canvas.toDataURL('image/jpeg');
            previewer.classList.add('selected');
        } catch (error) {
            console.error('Failed on image-processing: ', error);
        }
    }



});

/* Code from video "Tips & Trix - Den "kompletta" frontendguiden (G/VG) */
const dropdowns = document.querySelectorAll('[data-type="dropdown"]')
document.addEventListener('click', function (event) {
    let clickedDropdown = null

    dropdowns.forEach(dropdown => {
        const targetId = dropdown.getAttribute('data-target')
        const targetElement = document.querySelector(targetId)

        if (dropdown.contains(event.target)) {
            clickedDropdown = targetElement

            document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
                if (openDropdown !== targetElement) {
                    openDropdown.classList.remove('dropdown-show')
                }
            })
            targetElement.classList.toggle('dropdown-show')
        }
    })

    if (!clickedDropdown && !event.target.closest('.dropdown')) {
        document.querySelectorAll('.dropdown.dropdown-show').forEach(openDropdown => {
            openDropdown.classList.remove('dropdown-show')
        })
    }
})

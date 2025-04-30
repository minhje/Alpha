document.addEventListener('DOMContentLoaded', () => {
    const previewSize = 150;

    // Open modal
    const modalButtons = document.querySelectorAll('[data-modal="true"]');
    modalButtons.forEach(button => {
        button.addEventListener('click', async () => {
            const modalTarget = button.getAttribute('data-target');
            const modal = document.querySelector(modalTarget);

            if (modal) {
                modal.style.display = 'flex';

                // Hämta projekt-ID från data-attribut (endast för Edit-modal)
                const projectId = button.getAttribute('data-id');
                if (projectId) {
                    try {
                        const response = await fetch(`/Projects/Edit?id=${projectId}`);

            if (response.ok) {
                const html = await response.text(); // hämta HTML istället
                modal.innerHTML = html;

    // Initiera WYSIWYG efter att ny HTML är laddad
    initWysiwygEditor(modal);
} else {
    console.error('Failed to fetch project partial view');
}

                        //const response = await fetch(`/Projects/Edit?id=${projectId}`);
                        //if (response.ok) {
                        //    const projectData = await response.json();

                        //    // Fyll formuläret med projektdata
                        //    modal.querySelector('#edit-ProjectName').value = projectData.projectName || '';
                        //    modal.querySelector('#edit-Description').value = projectData.description || '';
                        //    modal.querySelector('#edit-StartDate').value = projectData.startDate || '';
                        //    modal.querySelector('#edit-EndDate').value = projectData.endDate || '';
                        //    modal.querySelector('#edit-Budget').value = projectData.budget || '';
                        //    modal.querySelector('#edit-SelectedClientId').value = projectData.selectedClientId || '';

                        //    //modal.querySelector('#edit-SelectedClientIds').value = projectData.selectedClientIds || '';
                        //} else {
                        //    console.error('Failed to fetch project data');
                        //}
                    } catch (error) {
                        console.error('Error fetching project data:', error);
                    }
                }

                // Initiera klient-sökning och WYSIWYG-redigering
                //initClientSearch(modal);
                initWysiwygEditor(modal);
            }
        });
    });

    // Close modal
    const closeButtons = document.querySelectorAll('[data-close="true"]');
    closeButtons.forEach(button => {
        button.addEventListener('click', () => {
            const modal = button.closest('.modal');

            if (modal) {
                modal.style.display = 'none';

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

    //function initClientSearch(modal) {
    //    const clientSearchInput = modal.querySelector('.form-tag-input');
    //    const clientSearchResults = modal.querySelector('.search-results');
    //    const selectedClientIds = modal.querySelector('[name="SelectedClientIds"]');

    //    if (clientSearchInput && clientSearchResults && selectedClientIds) {
    //        initTagSelector({
    //            containerId: clientSearchInput.closest('.form-tag-select').id,
    //            inputId: clientSearchInput.id,
    //            resultsId: clientSearchResults.id,
    //            searchUrl: (query) => `/Clients/SearchClients?term=${encodeURIComponent(query)}`,
    //            displayProperty: 'clientName',
    //            tagClass: 'client-tag',
    //            emptyMessage: 'No clients found.',
    //            hiddenInputId: selectedClientIds.id,
    //            preselected: []
    //        });
    //    } else {
    //        console.error("One or more required elements for initTagSelector are missing.");
    //    }
    //}

    function initWysiwygEditor(modal) {
        const editor = modal.querySelector('.wysiwyg-editor');
        const toolbar = modal.querySelector('.wysiwyg-toolbar');
        const textarea = modal.querySelector('textarea');

        if (editor && toolbar && textarea) {
            const quill = new Quill(editor, {
                modules: {
                    syntax: true,
                    toolbar: '#' + toolbar.id
                },
                placeholder: 'Enter a description',
                theme: "snow"
            });

            quill.on('text-change', () => {
                textarea.value = quill.root.innerHTML;
            });
        }
    }

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
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        form.addEventListener('submit', async (e) => {
            e.preventDefault();
            clearErrorMessages(form);

            const formData = new FormData(form);

            try {
                const res = await fetch(form.action, {
                    method: 'post',
                    body: formData
                });

                if (res.ok) {
                    const modal = form.closest('.modal');
                    if (modal) modal.style.display = "none";

                    window.location.reload()
                    console.log('Form submitted successfully');
                } else if (res.status === 400) {
                    const data = await res.json();

                    if (data.errors) {
                        Object.keys(data.errors).forEach(key => {
                            addErrorMessage(form, key, data.errors[key].join('\n'));
                        });
                    }
                }
            } catch {
                console.log('Failed to submit form');
            }
        });
    });

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
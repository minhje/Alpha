//function initTagSelector(config) {
//    let activeIndex = -1;
//    let selectedIds = [];

//    const tagContainer = document.getElementById(config.containerId);
//    const input = document.getElementById(config.inputId);
//    const results = document.getElementById(config.resultsId);
//    const selectedInputIds = document.getElementById(config.hiddenInputId);

//    // Kontrollera att alla element finns
//    if (!tagContainer || !input || !results || !selectedInputIds) {
//        console.error("One or more required elements for initTagSelector are missing.");
//        return;
//    }

//    // Preselect any provided items
//    if (Array.isArray(config.preselected)) {
//        config.preselected.forEach(item => {
//            addTag(item);
//            selectedIds.push(item.id);
//        });
//        updateSelectedIdsInput();
//    }

//    input.addEventListener('focus', () => {
//        tagContainer.classList.add('focused');
//        results.classList.add('focused');
//    });

//    input.addEventListener('blur', () => {
//        setTimeout(() => {
//            tagContainer.classList.remove('focused');
//            results.classList.remove('focused');
//        }, 100);
//    });

//    input.addEventListener('input', () => {
//        const query = input.value.trim();
//        activeIndex = -1;

//        if (query.length === 0) {
//            results.style.display = 'none';
//            results.innerHTML = '';
//            return;
//        }

//        fetch(config.searchUrl(query))
//            .then(r => {
//                if (!r.ok) throw new Error('Network response was not ok');
//                return r.json();
//            })
//            .then(data => renderSearchResults(data))
//            .catch(error => {
//                console.error('Error fetching search results:', error);
//                results.innerHTML = `<div class="search-item">${config.emptyMessage || 'Error fetching results'}</div>`;
//                results.style.display = 'block';
//            });
//    });

//    function renderSearchResults(data) {
//        results.innerHTML = '';

//        if (data.length === 0) {
//            const noResult = document.createElement('div');
//            noResult.classList.add('search-item');
//            noResult.textContent = config.emptyMessage || 'No clients found';
//            results.appendChild(noResult);
//        } else {
//            data.forEach(item => {
//                if (!selectedIds.includes(item.id)) {
//                    const resultItem = document.createElement('div');
//                    resultItem.classList.add('search-item');
//                    resultItem.dataset.id = item.id;
//                    resultItem.innerHTML = `<span>${item[config.displayProperty]}</span>`;
//                    resultItem.addEventListener('click', () => addTag(item));
//                    results.appendChild(resultItem);
//                }
//            });
//        }

//        results.style.display = 'block';
//    }

//    function addTag(item) {
//        const id = item.id;
//        if (selectedIds.includes(id)) return;

//        const tag = document.createElement('div');
//        tag.classList.add(config.tagClass || 'tag');
//        tag.innerHTML = `<span>${item[config.displayProperty]}</span>`;

//        const removeBtn = document.createElement('span');
//        removeBtn.textContent = 'x';
//        removeBtn.classList.add('btn-remove');
//        removeBtn.dataset.id = id;
//        removeBtn.addEventListener('click', (e) => {
//            selectedIds = selectedIds.filter(i => i !== id);
//            tag.remove();
//            updateSelectedIdsInput();
//            e.stopPropagation();
//        });

//        tag.appendChild(removeBtn);
//        tagContainer.insertBefore(tag, input);

//        selectedIds.push(id);
//        updateSelectedIdsInput();

//        input.value = '';
//        results.innerHTML = '';
//        results.style.display = 'none';
//    }

//    function removeLastTag() {
//        const tags = tagContainer.querySelectorAll(`.${config.tagClass}`);
//        if (tags.length === 0) return;

//        const lastTag = tags[tags.length - 1];
//        const lastId = lastTag.querySelector('.btn-remove').dataset.id;

//        selectedIds = selectedIds.filter(id => id !== lastId);
//        lastTag.remove();
//        updateSelectedIdsInput();
//    }

//    function updateSelectedIdsInput() {
//        if (selectedInputIds) {
//            selectedInputIds.value = selectedIds.join(',');
//        }
//    }

//    function updateActiveItem(items) {
//        items.forEach(item => item.classList.remove('active'));
//        if (items[activeIndex]) {
//            items[activeIndex].classList.add('active');
//            items[activeIndex].scrollIntoView({ block: 'nearest' });
//        }
//    }

//    input.addEventListener('keydown', (e) => {
//        const items = results.querySelectorAll('.search-item');

//        switch (e.key) {
//            case 'ArrowDown':
//                e.preventDefault();
//                if (items.length > 0) {
//                    activeIndex = (activeIndex + 1) % items.length;
//                    updateActiveItem(items);
//                }
//                break;

//            case 'ArrowUp':
//                e.preventDefault();
//                if (items.length > 0) {
//                    activeIndex = (activeIndex - 1 + items.length) % items.length;
//                    updateActiveItem(items);
//                }
//                break;

//            case 'Enter':
//                e.preventDefault();
//                if (activeIndex >= 0 && items[activeIndex]) {
//                    items[activeIndex].click();
//                }
//                break;

//            case 'Backspace':
//                if (input.value === '') {
//                    removeLastTag();
//                }
//                break;
//        }
//    });
//}

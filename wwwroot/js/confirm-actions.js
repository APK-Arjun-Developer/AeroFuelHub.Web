/**
 * Shows a confirmation dialog before submit when form has data-confirm,
 * or before delete/logout actions (see defaults below).
 */
(function () {
    const DEFAULT_DELETE_MESSAGE =
        'Are you sure you want to delete this item? This action cannot be undone.';

    function getConfirmMessage(form) {
        const custom = form.getAttribute('data-confirm');
        if (custom) return custom;

        const action = (form.getAttribute('action') || '').toLowerCase();
        if (action.includes('delete')) {
            return DEFAULT_DELETE_MESSAGE;
        }

        return null;
    }

    document.addEventListener(
        'submit',
        function (e) {
            const form = e.target;
            if (!(form instanceof HTMLFormElement)) return;

            const message = getConfirmMessage(form);
            if (!message) return;

            if (!window.confirm(message)) {
                e.preventDefault();
                e.stopImmediatePropagation();
            }
        },
        true
    );
})();

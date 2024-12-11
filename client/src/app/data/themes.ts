export const themeData = [
	{
		title: 'Slate',
		image: 'https://upload.wikimedia.org/wikipedia/commons/thumb/1/1f/20240911_Lisa_MTV_VMAs_06_%28cropped%29.png/220px-20240911_Lisa_MTV_VMAs_06_%28cropped%29.png',
		description: 'Default stuff',
		theme_color_scheme: `:root {
                    --ar-primary: #586889;
                    --ar-primary-rgb: 88, 104, 137;
                    --ar-border-radius: 0rem;
                    --ar-link-color: #586889;
                    --ar-link-hover-color: #3f4f70;
                    --ar-body-font-family:'Montserrat',sans-serif;
                }

                .btn-primary {
                    --ar-btn-bg: #586889;
                    --ar-btn-border-color: #586889;
                    --ar-btn-hover-bg: #3f4f70;
                    --ar-btn-hover-border-color: #3f4f70;
                    --ar-btn-active-bg: #3f4f70;
                    --ar-btn-active-border-color: #3f4f70;
                    --ar-btn-disabled-bg: #586889;
                    --ar-btn-disabled-border-color: #586889;
                }

                .btn-outline-primary {
                    --ar-btn-color: #586889;
                    --ar-btn-border-color: #586889;
                    --ar-btn-hover-bg: #586889;
                    --ar-btn-hover-border-color: #586889;
                    --ar-btn-active-bg: #586889;
                    --ar-btn-active-border-color: #586889;
                    --ar-btn-disabled-color: #586889;
                    --ar-btn-disabled-border-color: #586889;
                }

                .accordion-button:not(.collapsed)::after {
                    --ar-accordion-btn-active-icon: url("data:image/svg+xml,%3csvg%20xmlns='http://www.w3.org/2000/svg'%20viewBox='0%200%2024%2024'%20fill='%23586889'%3e%3cpath%20d='M.5%206.3c.6-.6%201.6-.6%202.3%200l9.3%209.3%209.3-9.3c.6-.6%201.6-.6%202.3%200%20.6.6.6%201.6%200%202.3L13.3%2018.8c-.6.6-1.6.6-2.3%200L.8%208.5c-.9-.8-.9-1.6-.3-2.2z'/%3e%3c/svg%3e");
            }`
	},
];

import { Translations } from '../i18n';

const en: Translations = {
    // Navigation
    home: 'Home',
    about: 'About',
    faq: 'FAQ',
    login: 'Log In',

    // Common
    language: 'Language',
    theme: 'Theme',
    light: 'Light',
    dark: 'Dark',
    system: 'System',

    // About page
    aboutUs: 'About Us',
    aboutDescription: 'Learn more about our company and mission',

    // FAQ
    frequentlyAskedQuestions: 'Frequently Asked Questions',
    faqDescription: 'Find answers to common questions',
    searchFaqPlaceholder: 'Search FAQs...',

    // FAQ Items
    faqItems: [
        {
            question: 'What is FrenCircle?',
            answer: 'FrenCircle is a modern social platform designed to help you connect and collaborate with friends, family, and communities. We provide tools for sharing, communication, and building meaningful relationships in a safe digital environment.'
        },
        {
            question: 'How do I create an account?',
            answer: 'Creating an account is easy! Click on the "Sign Up" button, provide your email address, create a secure password, and verify your email. You can also sign up using your Google or Facebook account for faster registration.'
        },
        {
            question: 'Is FrenCircle free to use?',
            answer: 'Yes, FrenCircle offers a comprehensive free plan that includes basic features like creating profiles, joining circles, and connecting with friends. We also offer premium plans with advanced features like unlimited storage, priority support, and exclusive tools.'
        },
        {
            question: 'How do I join a circle?',
            answer: 'You can join circles in several ways: accept an invitation from a friend, search for public circles using keywords or categories, browse our featured circles directory, or create your own circle and invite others to join.'
        },
        {
            question: 'How do I reset my password?',
            answer: 'To reset your password, go to the login page and click "Forgot Password". Enter your email address, and we\'ll send you reset instructions. You can also change your password from your account settings if you\'re already logged in.'
        },
        {
            question: 'What privacy controls are available?',
            answer: 'FrenCircle provides comprehensive privacy controls. You can make your profile public or private, control who can message you, manage circle visibility, block unwanted users, and customize what information is shared with others.'
        },
        {
            question: 'How do I report inappropriate content?',
            answer: 'If you encounter inappropriate content, click the report button (flag icon) next to the content. Our moderation team reviews all reports within 24 hours. You can also block users and leave circles if needed.'
        },
        {
            question: 'Can I delete my account?',
            answer: 'Yes, you can permanently delete your account from your account settings. This action is irreversible and will remove all your data, posts, and connections. You can also temporarily deactivate your account if you prefer.'
        },
        {
            question: 'How do I contact customer support?',
            answer: 'You can reach our support team through the Help Center, email us at support@frencircle.com, or use the live chat feature available 24/7. Premium users receive priority support with faster response times.'
        },
        {
            question: 'What devices and browsers are supported?',
            answer: 'FrenCircle works on all modern web browsers (Chrome, Firefox, Safari, Edge) and devices (desktop, tablet, mobile). We also have dedicated mobile apps for iOS and Android available in their respective app stores.'
        }
    ],

    // General
    welcome: 'Welcome to FrenCircle',
    description: 'A modern web application built with Next.js and shadcn/ui',
    browseProducts: 'Browse our complete product catalog',
    latestProducts: 'Latest products added to our collection'
};

export default en;

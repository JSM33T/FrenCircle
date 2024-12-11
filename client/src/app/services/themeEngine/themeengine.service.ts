import { Renderer2, RendererFactory2, Injectable } from '@angular/core';
import { themeData } from '../../data/themes';
import { fontData } from '../../data/fonts';

@Injectable({
    providedIn: 'root',
})
export class ThemeEngineService {
    private renderer: Renderer2;

    constructor(private rendererFactory: RendererFactory2) {
        this.renderer = this.rendererFactory.createRenderer(null, null);
    }

    initheme() {
        let style = localStorage.getItem("theme-color-scheme") || themeData[0].theme_color_scheme;
        let fontSize = localStorage.getItem("theme-font-size") || 1;
        let fontFamily = localStorage.getItem("theme-font-family") || fontData[0].font_family;
        let fontLink = localStorage.getItem("theme-font-link") || fontData[0].font_link;


        const styleElement = document.getElementById('theme-data') as HTMLStyleElement;
        if (styleElement) {
            styleElement.remove();
        }
        const newStyleElement = document.createElement('style');
        newStyleElement.id = 'theme-data';
        newStyleElement.innerHTML = style;
        document.head.appendChild(newStyleElement);


        const fontElement = document.getElementById('font-data') as HTMLLinkElement;


        if (fontElement) {
            this.renderer.setAttribute(fontElement, 'href', fontLink);
        } else {
            console.error('Font link element with id "font-data" not found.');
        }

        let fontSizeElement = document.getElementById('font-size') as HTMLStyleElement;
        if (fontSizeElement) {
            fontSizeElement.remove();
        }
        fontSizeElement = document.createElement('style');
        fontSizeElement.id = 'theme-data';
        fontSizeElement.innerHTML = `:root{--ar-root-font-size:${fontSize}rem;--ar-body-font-family:${fontFamily}}`;
        document.head.appendChild(fontSizeElement);
    }

    applyFont(size: number) {
        localStorage.setItem('theme-font-size', size.toString())
        this.initheme()
    }
}

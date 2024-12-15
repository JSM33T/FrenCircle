export interface MediaButton {
    title: string;
    link: string;
  }
  
  export interface MediaLink {
    title: string;
    description?: string;
    buttons?: MediaButton[];
    url?: string;
  }
  
  export interface MediaItem {
    title: string;
    type: string;
    description: string;
    images?: string[];
    videos?: string[];
    links?: MediaLink[];
  }
  
  export type MediaCollection = MediaItem[];
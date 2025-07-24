import type { NextConfig } from "next";

const nextConfig: NextConfig = {
  /* config options here */
  experimental: {
    optimizePackageImports: ['@radix-ui'],
  },
  // Ensure Google Fonts are properly optimized
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'fonts.googleapis.com',
      },
      {
        protocol: 'https',
        hostname: 'fonts.gstatic.com',
      },
    ],
  },
};

export default nextConfig;

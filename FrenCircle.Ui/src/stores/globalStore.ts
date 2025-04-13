import { create } from "zustand";

type GlobalStore = {
  data: Record<string, any>;
  set: (key: string, value: any) => void;
  get: (key: string) => any;
  clear: (key: string) => void;
};

export const useGlobalStore = create<GlobalStore>((set, get) => ({
  data: {},
  set: (key, value) =>
    set((state) => ({ data: { ...state.data, [key]: value } })),
  get: (key) => get().data[key],
  clear: (key) =>
    set((state) => {
      const newData = { ...state.data };
      delete newData[key];
      return { data: newData };
    }),
}));

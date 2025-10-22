import { ChangeEvent, FC } from "react";

interface Option {
  id: number | string;
  name: string;
}

interface SelectGroupProps<T extends Option> {
  label: string;
  field: string;
  handleChange: (e: ChangeEvent<HTMLSelectElement>) => void;
  error?: string | undefined;
  touched?: boolean | undefined;
  handleBlur: (e: ChangeEvent<HTMLSelectElement>) => void;
  options: T[];
  optionKey: keyof T;
  optionLabel: keyof T;
  initialItemId?: number | null | string;
  onChange?: (selectedItem: number) => void;
}

const SelectGroup: FC<SelectGroupProps<Option>> = ({
  label,
  field,
  handleBlur,
  error,
  touched,
  handleChange,
  options,
  optionKey,
  optionLabel,
  initialItemId,
  onChange = () => {},
}) => {
  return (
    <>
      <div className="w-[300px] flex flex-col justify-end">
        <label
          htmlFor="countries"
          className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
        >
          {label}
        </label>
        <select
          onBlur={handleBlur}
          name={field}
          className={`bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500 ${
            error && touched
              ? "bg-red-50 border border-red-500 text-red-900 placeholder-red-700 text-sm rounded-lg focus:ring-red-500 focus:border-red-500 block w-full p-2.5 dark:bg-red-100 dark:border-red-400"
              : ""
          }`}
          onChange={(e) => {
            handleChange(e);
            onChange(parseInt(e.target.value));
          }}
        >
          {options.map((item) => (
            <option
              key={item[optionKey]}
              value={item[optionKey]}
              selected={item[optionKey] === initialItemId}
            >
              {item[optionLabel]}
            </option>
          ))}
        </select>
        {error && touched && (
          <div className="mt-2 text-sm text-red-600 dark:text-red-500">
            {error}
          </div>
        )}
      </div>
    </>
  );
};

export default SelectGroup;

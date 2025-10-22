import { FC, useState, ReactNode } from "react";

interface Props {
  id: string | number;
  text: string;
  deleteFunc: (id: number | string) => void;
  children: ReactNode;
}

const ModalDelete: FC<Props> = ({ id, text, deleteFunc, children }) => {
  const [isModalOpen, setModalOpen] = useState(false);

  const handleDelete = async () => {
    setModalOpen(false);
    deleteFunc(id);
  };

  const handleOverlayClick = (event: React.MouseEvent<HTMLDivElement>) => {
    if (event.target === event.currentTarget) {
      setModalOpen(false);
    }
  };

  return (
    <>
      <div
        className="flex justify-center align-middle"
        onClick={(event) => {
          event.stopPropagation();
          setModalOpen(true);
        }}
      >
        {children}
      </div>

      {isModalOpen && (
        <div
          onClick={(event) => {
            event.stopPropagation();
            handleOverlayClick(event);
          }}
          className="fixed top-0 left-0 right-0 bottom-0 flex items-center justify-center bg-gray-900 bg-opacity-50 z-50"
        >
          <div className="relative bg-white rounded-lg shadow dark:bg-gray-700">
            <div className="p-6 text-center">
              <svg
                className="mx-auto mb-4 text-gray-400 w-12 h-12 dark:text-gray-200"
                xmlns="http://www.w3.org/2000/svg"
                fill="none"
                viewBox="0 0 20 20"
              >
                <path
                  stroke="currentColor"
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  strokeWidth="2"
                  d="M10 11V6m0 8h.01M19 10a9 9 0 1 1-18 0 9 9 0 0 1 18 0Z"
                />
              </svg>
              <h3 className="mb-5 text-lg font-normal text-gray-500 dark:text-gray-400">
                Are you sure you want to delete {text}?
              </h3>
              <button
                onClick={handleDelete}
                type="button"
                className="text-white bg-red-600 hover:bg-red-800 focus:ring-4 focus:outline-none focus:ring-red-300 dark:focus:ring-red-800 font-medium rounded-lg text-sm inline-flex items-center px-5 py-2.5 text-center mr-2"
              >
                Yes, I'm sure
              </button>
              <button
                onClick={() => setModalOpen(false)}
                type="button"
                className="text-gray-500 bg-white hover:bg-gray-100 focus:ring-4 focus:outline-none focus:ring-gray-200 rounded-lg border border-gray-200 text-sm font-medium px-5 py-2.5 hover:text-gray-900 focus:z-10 dark:bg-gray-700 dark:text-gray-300 dark:border-gray-500 dark:hover:text-white dark:hover:bg-gray-600 dark:focus:ring-gray-600"
              >
                No, cancel
              </button>
            </div>
          </div>
        </div>
      )}
    </>
  );
};

export default ModalDelete;

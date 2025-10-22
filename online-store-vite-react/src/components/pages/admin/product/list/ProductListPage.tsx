import React, { useEffect, useState } from "react";
import http_common from "../../../../../http_common.ts";
import ModalDelete from "../../../../../common/admin/ModalDelete.tsx";
import { useNavigate } from "react-router-dom";
import PaginationGroup from "../../../../../common/admin/PaginationGroup.tsx";
import { IFilter, IPaginatedList } from "../../../../../entities/Filter.ts";
import { IProduct } from "../../../../../entities/Product.ts";
import CreateButton from "../../../../../common/admin/CreateButtonGroup.tsx";
import { ICategory } from "../../../../../entities/Category.ts";
import useSort from "../../../../../hooks/useSort.ts";
import useSearch from "../../../../../hooks/useSearch.ts";
import usePage from "../../../../../hooks/usePage.ts";
import { Form, Formik } from "formik";
import SearchGroup from "../../../../../common/admin/SearchGroup.tsx";
import SelectGroup from "../../../../../common/admin/SelectGroup.tsx";
import { toast } from "react-toastify";
import useFilterQueryString from "../../../../../hooks/useFilterQueryString.ts";
import { IoIosArrowDown, IoIosArrowUp } from "react-icons/io";
import parse from "html-react-parser";
import SortableHeader from "../../../../../common/admin/SortableHeader.tsx";
import { BsFillTrashFill } from "react-icons/bs";
import { FaEdit } from "react-icons/fa";

interface SortOption {
  id: number | string;
  name: string;
  label: string;
}

function ProductsList() {
  const sortOptions = [
    { id: 0, name: "Id", label: "Id" },
    { id: 1, name: "Name", label: "Name" },
    { id: 3, name: "Price", label: "Price" },
    { id: 4, name: "Discount", label: "Discount" },
    { id: 5, name: "Rating", label: "Rating" },
  ];

  const sortDirections = [
    { id: 0, name: "asc", label: "Ascending" },
    { id: 1, name: "desc", label: "Descending" },
  ];

  const [productsPaginatedList, setProductsPaginatedList] = useState<
    IPaginatedList<IProduct>
  >({ totalPages: 0, items: [] });
  const [categories, setCategories] = useState<SortOption[]>([]);
  const [expandedProductId, setExpandedProductId] = useState<number | null>(
    null,
  );

  const navigate = useNavigate();

  const { convertFilterToQueryString } = useFilterQueryString();

  useEffect(() => {
    fetchData(initialFilterValues);
    fetchCategories();
  }, []);

  const fetchCategories = async () => {
    await http_common
      .get<ICategory[]>("api/Categories")
      .then((resp) => {
        const options: SortOption[] = resp.data.map((category: any) => ({
          id: category.id,
          name: category.name,
          label: category.name,
        }));
        setCategories(options);
      })
      .catch((error) => {
        toast.error("Failed to fetch categories data");
        console.error("Error fetching sort categories:", error);
      });
  };

  const queryParams = new URLSearchParams(location.search);

  const [initialFilterValues, setInitialFilterValues] = useState<IFilter>({
    pageNumber: parseInt(queryParams.get("page") ?? "1"),
    pageSize: 10,
    searchTerm: queryParams.get("search") ?? "",
    sortBy: queryParams.get("sort") ?? "Id",
    sortDirection: queryParams.get("direction") ?? "asc",
    sorting: queryParams.get("sorting") ?? "",
  });

  const fetchData = async (values: IFilter) => {
    await http_common
      .get<IPaginatedList<IProduct>>(
        `api/Products/getByFilter?${convertFilterToQueryString(values)}`,
      )
      .then((resp) => {
        setProductsPaginatedList(resp.data);
        setExpandedProductId(null);
      })
      .catch((error) => {
        toast.error("Failed to fetch products data");
        console.error("Error fetching Product data:", error);
      });
  };

  const { handleSortChange } = useSort({ setInitialFilterValues, fetchData });

  const handleCategoryChange = (selectedItem: number) => {
    setInitialFilterValues((prevValues) => {
      const updatedValues = {
        ...prevValues,
        sorting: categories[selectedItem].id.toString(),
      };
      fetchData(updatedValues);
      return updatedValues;
    });
  };

  const handleDelete = async (id: number | string) => {
    await http_common
      .delete(`api/Products/${id}`)
      .then(async () => {
        toast.success("Product deleted successfully");
        await fetchData(initialFilterValues);
      })
      .catch((error) => {
        toast.error("Failed to delete product");
        console.error("Error deleting category:", error);
      });
  };

  const { handleSearchChange } = useSearch({
    setInitialFilterValues,
    fetchData,
  });
  const { handlePageChange } = usePage({ setInitialFilterValues, fetchData });

  const findSortById = () => {
    const selectedSortByName = initialFilterValues.sortBy;
    const foundSortOption = sortOptions.find(
      (option) => option.name === selectedSortByName,
    );
    return foundSortOption ? foundSortOption.id : null;
  };

  const toggleExpandProduct = (productId: number) => {
    setExpandedProductId((prevExpandedProductId) =>
      prevExpandedProductId === productId ? null : productId,
    );
  };

  return (
    <>
      <div className="p-5 sm:ml-64">
        <div>
          <Formik
            initialValues={initialFilterValues}
            onSubmit={fetchData}
            enableReinitialize={true}
          >
            {({ handleChange, errors, touched, handleBlur }) => (
              <Form className="flex gap-5">
                <SearchGroup
                  onSearch={handleSearchChange}
                  label="Search Products"
                  value={initialFilterValues.searchTerm}
                ></SearchGroup>
                <CreateButton label={"Add Product"}></CreateButton>
                <SelectGroup
                  label="Sort by"
                  field="sortBy"
                  onChange={(selectedItem) => {
                    const selectedSortOption = sortOptions.find(
                      (option) => option.id === selectedItem,
                    );
                    if (selectedSortOption) {
                      handleSortChange({
                        ...initialFilterValues,
                        sortBy: selectedSortOption.name,
                      });
                    }
                  }}
                  handleChange={handleChange}
                  error={errors.sortBy}
                  touched={touched.sortBy}
                  handleBlur={handleBlur}
                  options={sortOptions}
                  optionKey="id"
                  optionLabel="name"
                  initialItemId={findSortById()}
                ></SelectGroup>
                <SelectGroup
                  label="Sort direction"
                  field="sortDirection"
                  onChange={(selectedItem) => {
                    const selectedDirection = sortDirections.find(
                      (dir) => dir.id === selectedItem,
                    );
                    if (selectedDirection) {
                      handleSortChange({
                        ...initialFilterValues,
                        sortDirection: selectedDirection.name,
                      });
                    }
                  }}
                  handleChange={handleChange}
                  error={errors.sortDirection}
                  touched={touched.sortDirection}
                  handleBlur={handleBlur}
                  options={sortDirections}
                  optionKey="id"
                  optionLabel="name"
                  initialItemId={
                    sortDirections.find(
                      (dir) => dir.name === initialFilterValues.sortDirection,
                    )?.id ?? null
                  }
                ></SelectGroup>
                <SelectGroup
                  label="Choose category"
                  field="sorting"
                  onChange={(selectedItem) => {
                    handleCategoryChange(selectedItem);
                  }}
                  handleChange={handleChange}
                  error={errors.sorting}
                  touched={touched.sorting}
                  handleBlur={handleBlur}
                  options={categories}
                  optionKey="id"
                  optionLabel="name"
                  initialItemId={
                    categories.find(
                      (category) =>
                        category.id.toString() === initialFilterValues.sorting,
                    )?.id ?? null
                  }
                ></SelectGroup>
              </Form>
            )}
          </Formik>
        </div>
        <div className="flex-grow overflow-hidden rounded-lg border border-gray-200 shadow-md mt-5">
          <div className="overflow-x-auto">
            <table className="w-full border-collapse bg-white text-left text-sm text-gray-500">
              <thead className="bg-gray-50">
                <tr>
                  <th className="px-4 sm:px-6 py-3 sm:py-4 font-medium text-gray-900"></th>
                  <SortableHeader
                    sortOptions={sortOptions}
                    initialFilterValues={initialFilterValues}
                    handleSortChange={handleSortChange}
                  />
                  <th className="px-4 sm:px-6 py-3 sm:py-4 font-medium text-gray-900 w-1/5">
                    Actions
                  </th>
                </tr>
              </thead>
              <tbody className="divide-y divide-gray-100 border-t border-gray-100">
                {productsPaginatedList.items.map((product) => (
                  <React.Fragment key={product.id}>
                    <tr
                      className="hover:bg-gray-50 cursor-pointer"
                      onClick={() => toggleExpandProduct(product.id)}
                    >
                      <td className="px-6 py-4">
                        {expandedProductId === product.id ? (
                          <IoIosArrowUp size={20} />
                        ) : (
                          <IoIosArrowDown size={20} />
                        )}
                      </td>
                      <th className="px-6 py-4">{product.id}</th>
                      <td className="flex gap-3 px-6 py-4 font-normal text-gray-900">
                        {product.images.length > 0 && (
                          <div className="relative h-10 w-10">
                            <img
                              className="h-10 w-10 rounded-full object-contain object-center max-w-none mr-5"
                              src={`${http_common.getUri()}/images/300_${
                                product.images[0].image
                              }`}
                              alt=""
                            />
                          </div>
                        )}
                        <div className="text-sm">
                          <div className="text-gray-400">{product.title}</div>
                        </div>
                      </td>
                      <td className="px-6 py-4">{product.price}</td>
                      <td className="px-6 py-4">{product.discount}</td>
                      <td className="px-6 py-4">{product.rating}</td>
                      <td className="px-6 py-4">
                        <div className="flex justify-end gap-4">
                          <ModalDelete
                            id={product.id}
                            text={product.title}
                            deleteFunc={handleDelete}
                          >
                            <button type="button">
                              <BsFillTrashFill size={20} />
                            </button>
                          </ModalDelete>
                          <button
                            type="button"
                            onClick={() => navigate(`update/${product.id}`)}
                          >
                            <FaEdit size={20} />
                          </button>
                        </div>
                      </td>
                    </tr>
                    {expandedProductId === product.id && (
                      <tr>
                        <td colSpan={8} className="px-6 py-4">
                          <div className="flex justify-between flex-wrap gap-5">
                            {product.images.map((image, index) => (
                              <div
                                className="relative w-60 h-40 border border-gray-300 bg-gray-50 rounded-lg overflow-hidden flex items-center justify-center"
                                key={index}
                              >
                                <img
                                  src={`${http_common.getUri()}/images/300_${
                                    image.image
                                  }`}
                                  alt={`Uploaded ${index}`}
                                  className="object-contain w-full h-full p-3"
                                />
                              </div>
                            ))}
                          </div>
                          <p className="font-medium text-gray-900 sm:py-3 text-lg">
                            Description
                          </p>
                          <p>{parse(product.description)}</p>
                          <div className="sm:py-3 flex gap-3">
                            <button
                              onClick={() => navigate(`update/${product.id}`)}
                              className="text-white bg-blue-600 hover:bg-blue-800 flex gap-2 items-center focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-4 py-2.5 dark:bg-blue-600 dark:hover:bg-blue-700 focus:outline-none dark:focus:ring-blue-800"
                            >
                              <FaEdit size={20} />
                              Edit
                            </button>
                            <ModalDelete
                              id={product.id}
                              text={product.title}
                              deleteFunc={handleDelete}
                            >
                              <button
                                type="button"
                                className="text-white bg-red-600 hover:bg-red-800 flex gap-2 items-center focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-4 py-2.5 dark:border-red-500 dark:hover:bg-red-700 focus:outline-none dark:focus:ring-red-800"
                              >
                                <BsFillTrashFill size={20} />
                                Delete
                              </button>
                            </ModalDelete>
                          </div>
                        </td>
                      </tr>
                    )}
                  </React.Fragment>
                ))}
              </tbody>
            </table>
          </div>
        </div>
        <PaginationGroup
          totalPages={productsPaginatedList.totalPages}
          onPageChange={handlePageChange}
          currentPage={initialFilterValues.pageNumber}
        ></PaginationGroup>
      </div>
    </>
  );
}

export default ProductsList;

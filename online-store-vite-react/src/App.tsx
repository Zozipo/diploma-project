import "./App.css";
import { Route, Routes } from "react-router-dom";
import Loader from "./common/styled/Loader.tsx";
import HomePage from "./components/pages/home/HomePage.tsx";
import CategorySelectionPage from "./components/pages/categorySelection/categorySelectionPage.tsx";
import ProductDetail from "./components/product/product_detail/Product_Detail.tsx";
import OrderPage from "./components/pages/order/OrderPage.tsx";
import UsersListPage from "./components/pages/admin/users/list/UsersListPage.tsx";
import UsersUpdatePage from "./components/pages/admin/users/update/UsersUpdatePage.tsx";
import CategoryList from "./components/pages/admin/category/list/CategoriesListPage.tsx";
import CategoriesCreatePage from "./components/pages/admin/category/create/CategoriesCreatePage.tsx";
import CategoriesUpdatePage from "./components/pages/admin/category/update/CategoriesUpdatePage.tsx";
import ProductList from "./components/pages/admin/product/list/ProductListPage.tsx";
import ProductsCreatePage from "./components/pages/admin/product/create/ProductsCreatePage.tsx";
import ProductsUpdatePage from "./components/pages/admin/product/update/ProductsUpdatePage.tsx";
import ProfilePage from "./components/pages/profile/profile/ProfilePage.tsx";
import ComprasionProduct from "./components/product/product-comparison/ComparisonProduct.tsx";
import AdminLayout from "./components/containers/default/AdminLayout.tsx";
import { useSelector } from "react-redux";
import { IAuthUser } from "./entities/Auth.ts";
import ChangePasswordPage from "./components/pages/profile/change-password/ChangePasswordPage.tsx";
import WishListPage from "./components/pages/profile/wishlist/WishListPage.tsx";
import UserLayout from "./components/containers/default/UserLayout.tsx";
import MyOrdersPage from "./components/pages/profile/orders/MyOrdersPage.tsx";
import AboutUsPage from "./components/pages/about/AboutUsPage.tsx";
import ProductComments from "./components/product/prduct-comments/ProductComments.tsx";
import { useEffect, useState } from "react";
import ErrorPage from "./components/pages/Error/ErrorPage.tsx";
import { ToastContainer } from "react-toastify";
import http_common from "./http_common.ts";
import { IModeratingTable } from "./entities/ModeratingTable.ts";
import React from "react";

const tableComponents: {
  [key: string]: {
    list: React.ComponentType;
    create: React.ComponentType;
    update: React.ComponentType;
  };
} = {
  categories: {
    list: CategoryList,
    create: CategoriesCreatePage,
    update: CategoriesUpdatePage,
  },
  products: {
    list: ProductList,
    create: ProductsCreatePage,
    update: ProductsUpdatePage,
  },
};

function App() {
  const { user, isAuth } = useSelector((store: any) => store.auth as IAuthUser);
  const [allModeratingTables, setAllModeratingTables] = useState<
    IModeratingTable[]
  >([]);

  const isAdmin = isAuth && user?.roles?.includes("Admin");
  const isModerator = isAuth && user?.roles?.includes("Moderator");

  const fetchModeratingTables = async () => {
    await http_common
      .get(`api/ModeratingTables`)
      .then((resp) => {
        setAllModeratingTables(resp.data);
      })
      .catch((error) => {
        console.error("Error fetching moderating tables data:", error);
      });
  };

  useEffect(() => {
    document.body.style.overflow = "auto";
    fetchModeratingTables();
  }, []);

  return (
    <>
      <Loader />
      <ToastContainer />
      <Routes location={location}>
        <Route path="*" element={<ErrorPage />} />
        <Route path={"/"} element={<HomePage />} />
        <Route path={"/comparison"} element={<ComprasionProduct />} />
        <Route path={"product/:id"} element={<ProductDetail />} />
        <Route path={"Reviews/:id"} element={<ProductComments />} />
        <Route path={"aboutus"} element={<AboutUsPage />} />
        <Route path={"/"} element={<UserLayout />}>
          <Route
            path={"categories/:name"}
            element={<CategorySelectionPage />}
          />
        </Route>
        <Route path={"order"} element={<OrderPage />} />

        {isAuth && (
          <>
            <Route path={"profile"} element={<ProfilePage />} />
            <Route
              path={"/profile/change-password"}
              element={<ChangePasswordPage />}
            />
            <Route path={"/profile/wishlist"} element={<WishListPage />} />
            <Route path={"/profile/myorders"} element={<MyOrdersPage />} />
          </>
        )}

        {(isAdmin || isModerator) && (
          <Route path={"/admin"} element={<AdminLayout />}>
            <Route path={"users"} element={<UsersListPage />} />
            <Route path={"users/update/:id"} element={<UsersUpdatePage />} />

            {allModeratingTables.map((table) => {
              const hasPermission =
                isAdmin || user?.tablePermissions?.includes(table.tableName);
              if (!hasPermission) return null;

              const tableName = table.tableName.toLowerCase();
              const components = tableComponents[tableName];

              if (!components) return null;

              return (
                <React.Fragment key={table.id}>
                  <Route
                    path={tableName}
                    element={React.createElement(components.list)}
                  />
                  <Route
                    path={`${tableName}/create`}
                    element={React.createElement(components.create)}
                  />
                  <Route
                    path={`${tableName}/update/:id`}
                    element={React.createElement(components.update)}
                  />
                </React.Fragment>
              );
            })}
          </Route>
        )}
      </Routes>
    </>
  );
}

export default App;

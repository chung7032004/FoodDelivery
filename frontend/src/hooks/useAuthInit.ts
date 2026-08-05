import { useEffect, useState } from "react";
import { authService } from "@/services/auth.service";

export function useAuthInit(){
    const [isLoading, setIsLoading] = useState(true);
    const [isAuthenticated, setIsAuthenticated] = useState(false);

    useEffect(()=>{
        authService.refresh()
        .then(()=>{setIsAuthenticated(true)})
        .catch(()=>{setIsAuthenticated(false)})
        .finally(()=>{setIsLoading(false)})
    },[]);
    return{ isLoading, isAuthenticated};
}